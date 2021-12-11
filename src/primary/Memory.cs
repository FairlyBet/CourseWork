using System.Linq;
using System.Collections.Generic;

namespace CourseWork
{
    public class Memory
    {
        private readonly uint _size;
        private readonly LinkedList<MemoryBlock> _blocks;

        public MemoryBlock[] Blocks => _blocks.ToArray();


        public Memory(in uint size)
        {
            _size = size;
            _blocks = new();
            _blocks.AddFirst(new MemoryBlock(BlockState.Empty, 0, size));
        }

        public bool TryAddProcces(Process process)
        {
            var block = _blocks.Where(x => x.State == BlockState.Empty && x.Size >= process.Size)
                               .MaxBy(x => x.Size);
            if (block != null)
            {
                OccupyBlock(process, block);
                return true;
            }
            return false;
        }

        private void OccupyBlock(in Process process, in MemoryBlock block)
        {
            var node = _blocks.Find(block);
            MemoryBlock newBlock = new(BlockState.Occupied, block.Address, process.Size);
            _blocks.AddBefore(node, newBlock);
            process.Location = newBlock;
            if (block.Size > process.Size)
            {
                MemoryBlock reducedBlock = new(BlockState.Empty,
                    block.Address + process.Size, block.Size - process.Size);
                _blocks.AddAfter(node, reducedBlock);
            }
            _blocks.Remove(node);
        }

        public void DeleteProcess(in Process process)
        {
            if (process.Location != null && _blocks.Contains(process.Location))
            {
                ReleaseBlock(process.Location);
            }
        }

        private void ReleaseBlock(in MemoryBlock block)
        {
            var node = _blocks.Find(block);
            node = UniteWithNext(node);
            node = UniteWithPrevious(node);
            MemoryBlock newBlock = new(BlockState.Empty, node.Value.Address, node.Value.Size);
            _blocks.AddBefore(node, newBlock);
            _blocks.Remove(node);
        }

        private LinkedListNode<MemoryBlock> UniteWithNext(LinkedListNode<MemoryBlock> node)
        {
            if (node.Next != null && node.Next.Value.State == BlockState.Empty)
            {
                MemoryBlock newBlock = new(BlockState.Empty,
                    node.Value.Address, node.Value.Size + node.Next.Value.Size);
                _blocks.AddBefore(node, newBlock);
                _blocks.Remove(node.Next);
                _blocks.Remove(node);
                node = _blocks.Find(newBlock);
            }
            return node;
        }

        private LinkedListNode<MemoryBlock> UniteWithPrevious(LinkedListNode<MemoryBlock> node)
        {
            if (node.Previous != null && node.Previous.Value.State == BlockState.Empty)
            {
                MemoryBlock newBlock = new(BlockState.Empty,
                    node.Previous.Value.Address, node.Value.Size + node.Previous.Value.Size);
                _blocks.AddBefore(node.Previous, newBlock);
                _blocks.Remove(node.Previous);
                _blocks.Remove(node);
                node = _blocks.Find(newBlock);
            }
            return node;
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach (var item in _blocks)
            {
                result += $"[{(double)item.Size / _size:p}, {item.State}]\n";
            }
            return result;
        }
    }
}
