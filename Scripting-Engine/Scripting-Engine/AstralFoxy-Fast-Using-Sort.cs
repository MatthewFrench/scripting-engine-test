// benchmarks:
//
//     quickusings.hoist:
//         1 MiB:  26 μs
//         10 MiB: 270 μs
//
//     equivalent-regex:
//         10 MiB: 72ms (270x)
using System;
using System.Collections.Generic;

static class QuickUsings
{

    // note: this method assumes complete and total ownership of `code` and will modify it! once you pass a string to
    // this method, you must drop all references to it within your code.
    public static unsafe Tuple<List<string>, string> Hoist(string code)
    {
        const string prefix = "\nusing ";

        fixed (char* text   = code)
        fixed (char* needle = prefix)
        {
            var headers          = new List<string>();          // the list of using statements we collect from `code`
            var codeLength       = code.Length;                 // cached length of `code`
            var codeEnd          = &text[codeLength];           // address at which `code` ends
            var prefixLength     = prefix.Length;               // cached length of `prefix`
            var prefixByteLength = prefixLength * sizeof(char); // the byte length of `prefix`
            var searchEnd        = codeLength - prefixLength;   // address at which no more possible instances of the prefix are possible (a full prefix before the end of the string)

            for (var i = 0; i < searchEnd; i++)
            {
                // search for "\nusing ...;"
                if (Equals((byte*)&text[i], (byte*)needle, prefixByteLength))
                {
                    // find the statement bounds
                    var start  = i;
                    var offset = FindSemiColon(&text[i + prefixLength], codeEnd);

                    // this is a "\nusing " prefix without a terminating semi-colon, and is thus invalid code
                    if (offset == -1)
                        break;

                    // add this using statement to our store
                    var length    = offset + prefixLength;
                    var statement = new string(text, start, length);
                    headers.Add(statement);

                    // then, comment out the span. doing it this way has two benefits:
                    //     - it is zero-copy
                    //     - it preserves file-line-column information in debug symbols (with a fixed offset for line number)
                    text[start + 0] = '/';
                    text[start + 1] = '*';
                    text[start + length - 2] = '*';
                    text[start + length - 1] = '/';

                    // fast-forward our "using"s search
                    i = start + length;
                }
            }

            return Tuple.Create<List<string>, string>(headers, headers.Count == 0
                ? code
                : Build(headers, code));
        }
    }

    static unsafe string Build(List<string> headers, string body)
    {
        // calculate the required length of the resulting string
        var length = 0;

        foreach (var x in headers) length += x.Length;
        length += body.Length;


        // create the string
        var output     = new string('\0', length);
        var byteLength = length * sizeof(char);
        var i          = 0;


        // copy constituent parts into the output string
        fixed (char* destination = output)
        {
            foreach (var x in headers)
            {
                var l = x.Length;

                fixed (char* source = x)
                    Buffer.MemoryCopy(source, destination + i, byteLength - i * sizeof(char), l * sizeof(char));

                i += l;
            }

            fixed (char* source = body)
                Buffer.MemoryCopy(source, destination + i, byteLength - i * sizeof(char), body.Length * sizeof(char));
        }


        // we're done!
        return output;
    }

    static unsafe int FindSemiColon(char* start, char* end)
    {
        var current = start;

        while (current < end)
        {
            if (*current == ';')
                return (int)(current - start + 1);

            ++current;
        }

        return -1;
    }

    static unsafe bool Equals(byte* x, byte* y, int length)
    {
        var last   = x + length;
        var last32 = last - 32;

        // unrolled loop
        while (x < last32)
        {
            if (*(ulong*)x != *(ulong*)y)
                return false;

            if (*(ulong*)(x + 8) != *(ulong*)(y + 8))
                return false;

            if (*(ulong*)(x + 16) != *(ulong*)(y + 16))
                return false;

            if (*(ulong*)(x + 24) != *(ulong*)(y + 24))
                return false;

            x += 32;
            y += 32;
        }

        while (x < last)
        {
            if (*x != *y)
                return false;

            ++x;
            ++y;
        }

        return true;
    }
}