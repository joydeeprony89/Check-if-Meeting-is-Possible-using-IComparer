using System;
using System.Collections.Generic;
using System.Linq;

namespace Check_if_Meeting_is_Possible
{
    class Program
    {
        static void Main(string[] args)
        {
            MyCalender calender = new MyCalender();
            Console.WriteLine(calender.BookUsingIComparer(20, 29));
            Console.WriteLine(calender.BookUsingIComparer(13, 22));
            Console.WriteLine(calender.BookUsingIComparer(44, 50));
            Console.WriteLine(calender.BookUsingIComparer(1, 7));
            Console.WriteLine(calender.BookUsingIComparer(2, 10));
            Console.WriteLine(calender.BookUsingIComparer(14, 20));
            Console.WriteLine(calender.BookUsingIComparer(19, 25));
            Console.WriteLine(calender.BookUsingIComparer(36, 42));
            Console.WriteLine(calender.BookUsingIComparer(45, 50));
            Console.WriteLine(calender.BookUsingIComparer(18, 27));
            var meetings = new int[5][] { new int[2] { 1, 3 }, new int[2] { 4, 6 }, new int[2] { 8, 9 }, new int[2] { 10, 12 }, new int[2] { 13, 15 } };
            var newMeeting = new int[2] { 9, 11 }; // 7, 8
            Console.WriteLine(CanScheduleMeeting(meetings, newMeeting));
        }

        static bool CanScheduleMeeting(int[][] meetings, int[] newMeeting)
        {
            if (meetings == null || meetings.Length == 0) return true;
            int length = meetings.Length;
            int start = 0, end = 0;
            int newStart = newMeeting[0];
            int newEnd = newMeeting[1];

            int i;
            for (i = 0; i < length; i++)
            {
                start = meetings[i][0];
                end = meetings[i][1];
                if (end > newStart) break;
            }

            // if we reach at end of the array and there are no meetings which end before proposed start
            if (i > length) return true;

            // start times are same return false
            if (newStart == start) return false;

            // if proposed start time is in between scheduled meeting
            if(newStart > start && newStart < end)
            {
                return false;
            } // if proposed end time is in between scheduled meeting
            else if(newEnd > start)
            {
                return false;
            }

            return true;
        }
    }

    public class MyCalender
    {
        readonly List<int[]> meetings;
        List<(int, int)> hash;
        public MyCalender()
        {
            meetings = new List<int[]>();
            hash = new List<(int, int)>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool Book(int start, int end)
        {
            if (meetings.Count == 0)
            {
                meetings.Add(new int[] { start, end });
                return true;
            }

            // get the existing schedule list and sort
            var existing = meetings.Select(x => x).ToArray();
            Array.Sort(existing, (a,b)=> a[0].CompareTo(b[0]));
            int length = existing.Length;
            int i;
            int existingStart = 0;
            int existingEnd = 0;
            for (i = 0; i < length; i++)
            {
                existingStart = existing[i][0];
                existingEnd = existing[i][1];
                if (existingEnd > start) break;
            }

            // if we reach at end of the array and there are no meetings which end before proposed start
            if (i == length)
            {
                meetings.Add(new int[] { start, end });
                return true;
            }

            // start time and existing start times are same return false
            if (start == existingStart) return false;

            // if new proposed start time is in between existing scheduled meeting time.
            if (start > existingStart && start < existingEnd) return false;
            else if (end > existingStart) return false; // if new proposed end time is in between existing scheduled meeting time.

            // good to schedule a meeting for the proposed time
            meetings.Add(new int[] { start, end });
            return true;
        }

        public bool BookUsingIComparer(int start, int end)
        {
            var range = (start, end);
            var index = hash.BinarySearch(range, new MeetingComperar());
            if (index < 0)
            {
                hash.Insert(~index, range);
                return true;
            }

            return false;
        }
    }

    public class MeetingComperar : IComparer<(int, int)>
    {
        public int Compare((int, int) x, (int, int) y)
        {
            if (x.Item2 <= y.Item1)
            {
                return -1;
            }
            if (y.Item2 <= x.Item1)
            {
                return 1;
            }
            return 0;
        }
    }
}
