using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDiff
{
    public class Result
    {

        public static Result Create(Bitmap image, IEnumerable<Rectangle> boundingBoxes)
        {
            return new Result()
            {
                Image = image,
                BoundingBoxes = boundingBoxes
            };
        }

        public System.Drawing.Bitmap Image { get; set; }

        public IEnumerable<Rectangle> BoundingBoxes{ get; set; }

        public bool IsSimilar
        {
            get
            {
                return BoundingBoxes.Count() == 0;
            }
        }

        /// <summary>
        /// For more options, use Image.Save
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            Image.Save(filename);
        }

        public void Save(string filename, System.Drawing.Imaging.ImageFormat format)
        {
            Image.Save(filename, format);
        }





    }

}

