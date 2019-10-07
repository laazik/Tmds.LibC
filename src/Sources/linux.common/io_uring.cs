using System.Runtime.InteropServices;

namespace Tmds.Linux
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct io_uring_sqe
    {
        [FieldOffset(0)]
        public byte opcode;
        [FieldOffset(1)]
        public byte flags;
        [FieldOffset(2)]
        public ushort ioprio;
        [FieldOffset(4)]
        public int fd;
        [FieldOffset(8)]
        public ulong off;
        [FieldOffset(16)]
        public ulong addr;
        [FieldOffset(24)]
        public uint len;
        [FieldOffset(28)]
        public int rw_flags;
        [FieldOffset(28)]
        public uint fsync_flags;
        [FieldOffset(28)]
        public ushort poll_events;
        [FieldOffset(28)]
        public uint sync_range_flags;
        [FieldOffset(28)]
        public uint msg_flags;
        [FieldOffset(32)]
        public ulong user_data;
        [FieldOffset(40)]
        public ushort buf_index;
        [FieldOffset(40)]
        public fixed ulong __pad2[3];
    };

    public struct io_uring_cqe
    {
        public ulong user_data;
        public int res;
        public uint flags;
    };

    public struct io_sqring_offsets
    {
        public uint head;
        public uint tail;
        public uint ring_mask;
        public uint ring_entries;
        public uint flags;
        public uint dropped;
        public uint array;
        public uint resv1;
        public ulong resv2;
    };

    public unsafe struct io_cqring_offsets
    {
        public uint head;
        public uint tail;
        public uint ring_mask;
        public uint ring_entries;
        public uint overflow;
        public uint cqes;
        public fixed ulong resv[2];
    };

    public unsafe struct io_uring_params
    {
        public uint sq_entries;
        public uint cq_entries;
        public uint flags;
        public uint sq_thread_cpu;
        public uint sq_thread_idle;
        public fixed uint resv[5];
        public io_sqring_offsets sq_off;
        public io_cqring_offsets cq_off;
    };

    public unsafe static partial class LibC
    {
        public static byte IOSQE_FIXED_FILE => 0;
        public static byte IOSQE_IO_DRAIN => 1;
        public static byte IOSQE_IO_LINK => 2;

        public static uint IORING_SETUP_IOPOLL => 0;
        public static uint IORING_SETUP_SQPOLL => 1;
        public static uint IORING_SETUP_SQ_AFF => 2;

        public static byte IORING_OP_NOP => 0;
        public static byte IORING_OP_READV => 1;
        public static byte IORING_OP_WRITEV => 2;
        public static byte IORING_OP_FSYNC => 3;
        public static byte IORING_OP_READ_FIXED => 4;
        public static byte IORING_OP_WRITE_FIXED => 5;
        public static byte IORING_OP_POLL_ADD => 6;
        public static byte IORING_OP_POLL_REMOVE => 7;
        public static byte IORING_OP_SYNC_FILE_RANGE => 8;
        public static byte IORING_OP_SENDMSG => 9;
        public static byte IORING_OP_RECVMSG => 10;

        public static uint IORING_FSYNC_DATASYNC => 1;

        public static ulong IORING_OFF_SQ_RING => 0;
        public static ulong IORING_OFF_CQ_RING => 0x8000000UL;
        public static ulong IORING_OFF_SQES => 0x10000000UL;

        public static uint IORING_SQ_NEED_WAKEUP => 1;

        public static uint IORING_ENTER_GETEVENTS => 0;
        public static uint IORING_ENTER_SQ_WAKEUP => 1;

        public static uint IORING_REGISTER_BUFFERS => 0;
        public static uint IORING_UNREGISTER_BUFFERS => 1;
        public static uint IORING_REGISTER_FILES => 2;
        public static uint IORING_UNREGISTER_FILES => 3;
        public static uint IORING_REGISTER_EVENTFD => 4;
        public static uint IORING_UNREGISTER_EVENTFD => 5;

        public static int io_uring_register(int fd, uint opcode, void* arg, uint nr_args)
        {
            return (int)syscall(__NR_io_uring_register, fd, opcode, arg, nr_args);
        }

        public static int io_uring_setup(uint entries, io_uring_params* p)
        {
            return (int)syscall(__NR_io_uring_setup, entries, p);
        }

        public static int io_uring_enter(int fd, uint to_submit, uint min_complete, uint flags, sigset_t* sig)
        {
            return (int)syscall(__NR_io_uring_enter, fd, to_submit, min_complete, flags, sig, NSIG / 8);
        }

        private static int __NR_io_uring_setup => 425;
        private static int __NR_io_uring_enter => 426;
        private static int __NR_io_uring_register => 427;
    }
}