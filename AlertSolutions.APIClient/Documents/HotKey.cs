using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AlertSolutions.API.Documents
{
    public class HotKey
    {
        private static Dictionary<char, string> _tagMap = new Dictionary<char, string>
        {
            { '0', "HotZero" },
            { '1', "HotOne" },
            { '2', "HotTwo" },
            { '3', "HotThree" },
            { '4', "HotFour" },
            { '5', "HotFive" },
            { '6', "HotSix" },
            { '7', "HotSeven" },
            { '8', "HotEight" },
            { '9', "HotNine" },
            { '*', "HotStar" },
            { '#', "HotPound" },
        };

        public char Key { get; private set; }
        public string Command { get; private set; }
        public string XferNumber { get; private set; }

        private HotKey(char key)
        {
            Key = key;
        }

        public static bool IsValidHotKey(char key)
        {
            return    ('0' <= key && key <= '9')
                      || (key == '*' || key == '#');
        }

        private static void Validate(char key)
        {
            if (!IsValidHotKey(key))
                throw new ArgumentException(key + " is not a valid hot key. Valid hot keys are 0-9, * and #.", "key");
        }

        public static HotKey CreateStop(char key)
        {
            Validate(key);
            return new HotKey(key) { Command = "STOP" };
        }

        public static HotKey CreateRepeat(char key)
        {
            Validate(key);
            return new HotKey(key) { Command = "REPEAT" };
        }

        public static HotKey CreateTransfer(char key, string phoneNumber)
        {
            Validate(key);
            return new HotKey(key) { Command = "DIALTO", XferNumber = phoneNumber };
        }

        public XElement ToXml()
        {
            return new XElement(_tagMap[Key],
                this.Command == "DIALTO" ? Command + " " + XferNumber
                                         : Command);
        }
    }
}