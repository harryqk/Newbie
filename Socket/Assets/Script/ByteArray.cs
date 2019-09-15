using System;
using System.IO;
using System.Text;

public class ByteArray
{

    public static ByteArray Create(long opcode)
    {
        return new ByteArray(opcode);
    }
    public static ByteArray Create(byte[] bytes)
    {
        return new ByteArray(bytes);
    }
    private MemoryStream innerStream;
    private BinaryReader reader;
    private BinaryWriter writer;
    public ByteArray()
    {
        innerStream = new MemoryStream();
        reader = new BinaryReader(innerStream);
        writer = new BinaryWriter(innerStream);
    }
    public ByteArray(long opcode)
    {
        innerStream = new MemoryStream();
        reader = new BinaryReader(innerStream);
        writer = new BinaryWriter(innerStream);
        WriteShort((short)opcode);
    }
    public ByteArray(byte[] bytes)
    {
        innerStream = new MemoryStream(bytes);
        reader = new BinaryReader(innerStream);
        writer = new BinaryWriter(innerStream);
    }

    public int Position
    {
        get
        {
            return (int)innerStream.Position;
        }
        set
        {
            innerStream.Position = value;
        }
    }
    public int Length
    {
        get
        {
            return (int)innerStream.Length;
        }
    }

    public int ReadByte()
    {
        return reader.ReadByte();
    }

    public bool ReadBool()
    {
        return reader.ReadByte() != 0;
    }
    public int ReadUShort()
    {
        return reader.ReadUInt16();
    }
    public int ReadShort()
    {
        return reader.ReadInt16();
    }
    public long ReadUInt()
    {
        return reader.ReadUInt32();
    }
    public int ReadInt()
    {
        return reader.ReadInt32();

    }
    public long ReadLong()
    {
        return reader.ReadInt64();
    }
    public float ReadFloat()
    {
        int rint = reader.ReadInt32();
        byte[] bytes = BitConverter.GetBytes(rint);
        return BitConverter.ToSingle(bytes, 0);
    }
    public double ReadDouble()
    {
        long rint = reader.ReadInt64();
        byte[] bytes = BitConverter.GetBytes(rint);
        return BitConverter.ToDouble(bytes, 0);
    }
    public byte[] ReadBytes()
    {
        int len = ReadInt();
        return reader.ReadBytes(len);
    }
    public string ReadString()
    {
        int len = ReadShort();
        byte[] bytes = reader.ReadBytes(len);
        return Encoding.UTF8.GetString(bytes);
    }
    public int ReadVarint32()
    {
        int result = 0;
        int offset = 0;
        for (; offset < 32; offset += 7)
        {
            int b = reader.ReadByte();
            if (b == -1)
            {
                throw new Exception("Varint32格式错误");
            }
            result |= (b & 0x7f) << offset;
            if ((b & 0x80) == 0)
            {
                return (int)result;
            }
        }
        // Keep reading up to 64 bits.
        for (; offset < 64; offset += 7)
        {
            int b = reader.ReadByte();
            if (b == -1)
            {
                throw new Exception();
            }
            if ((b & 0x80) == 0)
            {
                return (int)result;
            }
        }
        throw new Exception("Varint32格式错误");
    }
    long ReadRawVarint64SlowPath()
    {
        long result = 0;
        for (int shift = 0; shift < 64; shift += 7)
        {
            byte b = reader.ReadByte();
            result |= (long)(b & 0x7F) << shift;
            if ((b & 0x80) == 0)
            {
                return result;
            }
        }
        throw new Exception();
    }
    public ByteArray WriteByte(byte value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteBool(bool value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteUShort(ushort value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteShort(short value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteUInt(uint value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteInt(int value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteLong(long value)
    {
        writer.Write(value);
        return this;
    }
    public ByteArray WriteFloat(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        int rint = BitConverter.ToInt32(bytes, 0);
        writer.Write(rint);
        return this;
    }
    public ByteArray WriteDouble(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        long rint = BitConverter.ToInt64(bytes, 0);
        writer.Write(rint);
        return this;
    }
    public ByteArray WriteString(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        WriteShort((short)bytes.Length);
        writer.Write(bytes);
        return this;
    }

    internal ByteArray WriteRawVarint32(uint value)
    {
        // Optimize for the common case of a single byte value
        if (value < 128 && writer.BaseStream.Position < writer.BaseStream.Length)
        {
            writer.Write((byte)value);
            return this;
        }

        while (value > 127 && writer.BaseStream.Position < writer.BaseStream.Length)
        {
            writer.Write((byte)((value & 0x7F) | 0x80));
            value >>= 7;
        }
        while (value > 127)
        {
            writer.Write((byte)((value & 0x7F) | 0x80));
            value >>= 7;
        }
        if (writer.BaseStream.Position < writer.BaseStream.Length)
        {
            writer.Write((byte)value);
        }
        else
        {
            writer.Write((byte)value);
        }
        return this;
    }
    public ByteArray WriteBytes(byte[] bytes)
    {
        WriteInt(bytes.Length);
        writer.Write(bytes);
        return this;
    }

    public byte[] ToArray()
    {
        return innerStream.ToArray();
    }

    public void WriteTo(Stream stream)
    {
        innerStream.WriteTo(stream);
    }
}
