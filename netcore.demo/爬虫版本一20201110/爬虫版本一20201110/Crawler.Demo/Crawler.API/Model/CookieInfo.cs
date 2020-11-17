namespace Crawler.API.Model
{
    public class CookieInfo
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Domain default .instagram.com
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// default /
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Max-Age
        /// </summary>
        public string Expires { get; set; }

        /// <summary>
        /// Size
        /// </summary>

        public int Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool HttpOnly { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Secure { get; set; }
        /// <summary>
        /// default string.Empty
        /// </summary>
        public string SameSite { get; set; }
        /// <summary>
        /// default Medium
        /// </summary>
        public string Priority { get; set; }
    }
}
