#!/bin/bash
# getting the day number of the month (assuming you are in december...)

if [ -n "$1" ]
then
    DD=$1
else
    DD=$(date +%e | xargs)
fi

file="./src/Days/Day$DD.cs"
echo $file
if [ -f "$file" ]
then
    echo "Bad day to get creative... File already exists."
    exit
else
    cat <<EOF > $file
using System;

namespace AdventOfCode
{
    public class Day$DD : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;

            Console.WriteLine($"Day$DD:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;

            Console.WriteLine($"Day$DD:PartTwo: {answer}");
        }
    }
}
EOF

    echo "Hello! It's a new day!"
fi
