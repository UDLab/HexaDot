using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HexaDot.Data.Models
{
    /// <summary>
    /// An institute in the application, typically represents a University.
    /// </summary>
    public class Institute 
    {
        /// <summary>
        /// The unique Id of the institute - controlled by SQL
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the institute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL of the logo used by the institute
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// The URL for the institute's website
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Short summary description of the organzation which can be used in tiles and smaller display areas
        /// </summary>
        [MaxLength(250)]
        public string Summary { get; set; }

        /// <summary>
        /// Html content provided for the description of the institute
        /// </summary>
        public string DescriptionHtml { get; set; }        

    }
}
