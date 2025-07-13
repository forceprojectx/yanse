using SkiaSharp;

namespace yanse.Models
{
    /// <summary>
    /// Color data
    /// </summary>
    public class Yanse
    {
        /// <summary>
        /// <seealso cref="SKColor"/> to compare to the primary color of the camera image
        /// </summary>
        public required SKColor Color { get; set; }

        /// <summary>
        /// The <seealso cref="Yanse.Color"/> in HSV
        /// </summary>
        public required HSV Hsv { get; set; }

        /// <summary>
        /// English name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Chinese name
        /// </summary>
        public required string Chinese { get; set; }

        /// <summary>
        /// Pinyin representation of the color's name
        /// </summary>
        public required string Pinyin { get; set; }

        /// <summary>
        /// File name for the character stroke SVG image
        /// </summary>
        /// <remarks>strokes provided by https://github.com/skishore/makemeahanzi.git</remarks>
        public string HanziFileName => $"hanzi_{(int)Chinese[0]}_still.svg";

        /// <summary>
        /// factory method for the color objects
        /// </summary>
        /// <param name="name"></param>
        /// <param name="chinese"></param>
        /// <param name="pinyin"></param>
        /// <param name="hex">color in RGB hex format. EG: "#FFC0CB"</param>
        /// <returns></returns>
        private static Yanse Create(string name, string chinese, string pinyin, string hex)
        {
            var skColor = SKColor.Parse(hex);
            float h;
            float s;
            float v;
            skColor.ToHsv(out h, out s, out v);
            return new Yanse
            {
                Name = name,
                Chinese = chinese,
                Pinyin = pinyin,
                Color = skColor,
                Hsv = new HSV
                {
                    Hue = (float)Math.Round(h, 3),
                    Saturation = (float)Math.Round(s, 3),
                    Value = (float)Math.Round(v, 3)
                }
            };
        }
        /// <summary>
        /// List of colors
        /// </summary>
        /// <remarks>gold and silver should probably be part of of the extended set
        /// or removed. They won't be visually different from yellow/gray</remarks>
        public static List<Yanse> Colors = new List<Yanse>
    {
        Create("Red",     "红色",   "hóng sè",     "#FF0000"),
        Create("Orange",  "橙色",   "chéng sè",    "#FFA500"),
        Create("Yellow",  "黄色",   "huáng sè",    "#FFFF00"),
        Create("Green",   "绿色",   "lǜ sè",       "#008000"),
        Create("Blue",    "蓝色",   "lán sè",      "#0000FF"),
        Create("Purple",  "紫色",   "zǐ sè",       "#800080"),
        Create("Pink",    "粉色",   "fěn sè",      "#FFC0CB"),
        Create("Brown",   "棕色",   "zōng sè",     "#964B00"),
        Create("Gray",    "灰色",   "huī sè",      "#808080"),
        Create("Black",   "黑色",   "hēi sè",      "#000000"),
        Create("White",   "白色",   "bái sè",      "#FFFFFF"),
        Create("Gold",    "金色",   "jīn sè",      "#FFD700"),
        Create("Silver",  "银色",   "yín sè",      "#C0C0C0"),
        // extended colors
        Create("Cyan",       "青色",   "qīng sè",     "#00FFFF" ),
        Create("Maroon",     "栗色",   "lì sè",       "#800000" ),
        Create("Beige",      "米色",   "mǐ sè",       "#F5F5DC" ),
        Create("Indigo",     "靛色",   "diàn sè",     "#4B0082" ),
        Create("Magenta",    "品红",   "pǐn hóng",    "#FF00FF" ),
        Create("Lime",       "浅绿",   "qiǎn lǜ",     "#00FF00" ),
        Create("Teal",       "蓝绿",   "lán lǜ",      "#008080" ),
        Create("Navy",       "藏青",   "zàng qīng",   "#000080" ),
        Create("Sky Blue",   "天蓝色", "tiān lán sè", "#87CEEB" ),
        Create("Turquoise",  "青绿色", "qīng lǜ sè",  "#40E0D0" ),
        Create("Coral",      "珊瑚色", "shān hú sè",  "#FF7F50" ),
        Create("Olive",      "橄榄色", "gǎn lǎn sè",  "#808000" ),
    };

    }
}
