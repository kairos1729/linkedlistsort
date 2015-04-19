using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linkedlistsort
{
    class Program
    {
        private static int nops = 0;
        private static Random random = new Random();

        private class Node<T>
        {
            public Node<T> next;
            public T value;

            public static Node<T> FromSequence(IEnumerable<T> s)
            {
                return s.Reverse().Aggregate(
                    (Node<T>)null,
                    (a, v) => new Node<T> { value = v, next = a });
            }

            public IEnumerable<T> ToSequence()
            {
                var node = this;

                while (node != null)
                {
                    yield return node.value;
                    node = node.next;
                }
            }

            public override string ToString()
            {
                return string.Join(", ", this.ToSequence());
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                var list = Node<int>.FromSequence(RandomSequence(65536));

                Console.WriteLine("Start: " + list);
                Console.ReadLine();

                Sort(ref list, list, null);

                Console.WriteLine("Sorted: " + list);
                Console.WriteLine("nops: " + nops);
                Console.ReadLine();

                nops = 0;
            }
        }

        private static IEnumerable<int> RandomSequence(int n)
        {
            return Enumerable.Repeat(0, n).Select(_ => random.Next(n * 128));
        }

        static void Sort(ref Node<int> head, Node<int> pivot, Node<int> last)
        {
            // End the recursion
            if ((pivot == null) || (pivot.next == null) || (pivot == last))
            {
                return;
            }

            // Move all elements less than the head to before the head...
            var pivotValue = pivot.value;
            var previous = pivot;
            var node = pivot.next;

            do
            {
                nops += 1; // keep track of the time complexity

                var next = (node == last) ? node : node.next;

                if (node.value < pivotValue)
                {
                    previous.next = next;
                    node.next = head;
                    head = node;
                }
                else
                {
                    previous = node;
                }

                node = next;
            }
            while (node != last);

            // ...then sort both partitions recursively (so this is really log n space)
            Sort(ref head, head, pivot);
            Sort(ref pivot.next, pivot.next, last);
        }
    }
}
