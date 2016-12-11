using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nantuko.ManicEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nantuko.ManicEngine.Tests
{
    [TestClass()]
    public class BorderTests
    {
        [TestMethod()]
        public void CalculateBorderIdTest()
        {
            MapCordinate cordinate00 = new MapCordinate(0, 0);
            MapCordinate cordinate01 = new MapCordinate(0, 1);
            MapCordinate cordinate10 = new MapCordinate(1, 0);
            MapCordinate cordinate11 = new MapCordinate(1, 1);
            MapCordinate cordinate22 = new MapCordinate(2, 2);

            long x = Border.CalculateBorderId(cordinate00, cordinate00);
            long a = Border.CalculateBorderId(cordinate00, cordinate01);
            long b = Border.CalculateBorderId(cordinate00, cordinate10);
            long c = Border.CalculateBorderId(cordinate00, cordinate11);
            long d = Border.CalculateBorderId(cordinate01, cordinate10);
            long e = Border.CalculateBorderId(cordinate01, cordinate11);
            long f = Border.CalculateBorderId(cordinate10, cordinate11);
            long o = Border.CalculateBorderId(cordinate00, cordinate22);

            byte[] xr = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }.Reverse().ToArray();
            byte[] ar = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }.Reverse().ToArray();
            byte[] br = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 }.Reverse().ToArray();
            byte[] cr = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01 }.Reverse().ToArray();
            byte[] dr = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00 }.Reverse().ToArray();
            byte[] er = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01 }.Reverse().ToArray();
            byte[] fr = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01 }.Reverse().ToArray();

            long xt = BitConverter.ToInt64(xr, 0);
            long at = BitConverter.ToInt64(ar, 0);
            long bt = BitConverter.ToInt64(br, 0);
            long ct = BitConverter.ToInt64(cr, 0);
            long dt = BitConverter.ToInt64(dr, 0);
            long et = BitConverter.ToInt64(er, 0);
            long ft = BitConverter.ToInt64(fr, 0);

            if (x != 0) Assert.Fail("0.0 0.0 X");
            if (o != 0) Assert.Fail("0.0 2.2 O");

            if (xt != x) Assert.Fail("0.0 0.0 X");
            if (at != a) Assert.Fail("0.0 1.0 A");
            if (bt != b) Assert.Fail("0.0 0.1 B");
            if (ct != c) Assert.Fail("0.0 1.1 C");
            if (dt != d) Assert.Fail("0.1 1.0 D");
            if (et != e) Assert.Fail("0.1 1.1 F");
            if (ft != f) Assert.Fail("1.0 1.1 E");


        }
    }
}