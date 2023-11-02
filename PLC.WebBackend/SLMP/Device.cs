using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLMP
{
    /// <summary>
    /// This enum contains the type of supported device types.
    /// </summary>
    public enum DeviceType
    {
        Bit,
        Word
    }

    /// <summary>
    /// This enum encodes the supported devices that is available to operate on.
    /// </summary>
    public enum Device
    {
        /// <summary>
        /// Word
        /// </summary>
        D = 0xa8, // Data Register(D)
        W = 0xb4, // Link Register(W)
        R = 0xaf, // File Register(R)
        Z = 0xcc, // Index register(Z)
        ZR = 0xb0,// File Register(ZR) Hex 
        SD = 0xa9,// Specil Register(SD)
        TN = 0xc2,// Timer(T) Current Value(TN)
        LTN = 0x52, // Long Timer(LT) Current Value(LTN)
        STN = 0x0c8, // Retentive Timer(ST) Current Value(STN)
        LSTN = 0x05a, // Long Retentive Timer(LST) Current Value(LSTN)
        CN = 0xc5, // Counter(C) Current Value(CN)
        LCN = 0x56, // Long Counter(LC) Current Value(LCN)

        /// <summary>
        /// Bit
        /// </summary>
        X = 0x9c, // Input(X)
        Y = 0x9d, // Output(Y)
        M = 0x90, // Internal Relay(M)
        L = 0x92, // Latch Relay(L)
        F = 0x93, // Annunciator(F)
        V = 0x94, // Edge Relay(V)
        B = 0xa0, // Link Relay(B)
        SM = 0x91, // Special Relay(SM)
        TS = 0xc1, // Timer(T) Contact(TS)
        TC = 0xc0, // Timer(T) Coil(TC)
        LTS = 0x51, // Long Timer(LT) Contact(LTS)
        LTC = 0x50, // Long Timer(LT) Coil(LTC)
        STS = 0xc7, // Relentive Timer(ST) Contact(STS)
        STC = 0xc6, // Relentive Timer(ST) Coil(STC)
        LSTS = 0x59, // Long Relentive Timer(LST) Contact(LSTS)
        LSTC = 0x5a, // Long Relentive Timer(LST) Coil(LSTC)
        CS = 0xc4, // Counter(C) Contact(CS)
        CC = 0xc3, // Counter(C) Coil(CC)
        LCS = 0x55, // Long Counter(LC) Contact(LCS)
        LCC = 0x54, // LONG Counter(LC) Coil(LCC)
    }

    public class DeviceMethods
    {
        /// <summary>
        /// Gets the subcommand for a given `(Bit/Word)Device`.
        /// </summary>
        public static ushort GetSubcommand(Device device)
        {
            return DeviceMethods.GetDeviceType(device) switch
            {
                DeviceType.Bit => 0x0001,
                DeviceType.Word => 0x0000,
                _ => throw new ArgumentException("invalid device type provided"),
            };
        }

        /// <summary>
        /// This helper function will return either `DeviceType.Word` or `DeviceType.Bit`
        /// for a given `device`.
        /// </summary>
        /// <returns>DeviceType</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DeviceType GetDeviceType(Device device)
        {
            return device switch
            {
                Device.D => DeviceType.Word,
                Device.W => DeviceType.Word,
                Device.R => DeviceType.Word,
                Device.Z => DeviceType.Word,
                Device.ZR => DeviceType.Word,
                Device.SD => DeviceType.Word,
                Device.TN => DeviceType.Word,
                Device.LTN => DeviceType.Word,
                Device.STN => DeviceType.Word,
                Device.LSTN => DeviceType.Word,
                Device.CN => DeviceType.Word,
                Device.LCN => DeviceType.Word,

                Device.X => DeviceType.Bit,
                Device.Y => DeviceType.Bit,
                Device.M => DeviceType.Bit,
                Device.L => DeviceType.Bit,
                Device.F => DeviceType.Bit,
                Device.V => DeviceType.Bit,
                Device.B => DeviceType.Bit,
                Device.SM => DeviceType.Bit,
                Device.TS => DeviceType.Bit,
                Device.TC => DeviceType.Bit,
                Device.LTS => DeviceType.Bit,
                Device.LTC => DeviceType.Bit,
                Device.STS => DeviceType.Bit,
                Device.STC => DeviceType.Bit,
                Device.LSTS => DeviceType.Bit,
                Device.CS => DeviceType.Bit,
                Device.CC => DeviceType.Bit,
                Device.LCS => DeviceType.Bit,
                Device.LCC => DeviceType.Bit,



                _ => throw new ArgumentException("invalid device")
            };
        }

        /// <summary>
        /// Helper function to get a `Device` from a given string.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool FromString(string device, out Device value)
        {
            return Enum.TryParse<Device>(device, true, out value);
        }

        /// <summary>
        /// Helper function to parse strings in the form `{DeviceName}{DeviceAddress}`.
        /// </summary>
        /// <returns>Tuple<Device, ushort></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Tuple<Device, ushort> ParseDeviceAddress(string address)
        {
            Regex rx = new(@"([a-zA-Z]+)(\d+)");
            Match match = rx.Match(address);

            if (match.Groups.Count < 3)
                throw new ArgumentException($"couldn't parse device address: {address}");

            string sdevice = match.Groups[1].Value;
            string saddr = match.Groups[2].Value;

            if (!FromString(sdevice, out Device device)) throw new ArgumentException($"invalid device provided: {sdevice}");
            if (!UInt16.TryParse(saddr, out ushort uaddr)) throw new ArgumentException($"invalid address provided: {saddr}");

            return Tuple.Create((Device)device, uaddr);
        }
    }
}
