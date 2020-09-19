using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PdfEditor
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fileName = "teste";
            string filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";

            var programs = new Program();
            var listFieldNames = programs.ListFieldNames(filePath, fileName);
            if (listFieldNames == null || !listFieldNames.Any())
                return;

            programs.FillForm(filePath, fileName, listFieldNames);

            Console.ReadLine();
        }

        /// <summary>
        /// Get list of field names
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public IEnumerable<string> ListFieldNames(string filePath, string fileName)
        {
            var fullPathFile = $@"{filePath}\{fileName}.pdf";
            if (!FileExists(filePath, fileName)) return null;

            // create a new PDF reader based on the PDF template document
            PdfReader pdfReader = new PdfReader(fullPathFile);

            // field names available in the subject PDF
            Console.WriteLine("****** - FIELDS FROM PDF FILE - ******");
            var listFieldNames = pdfReader.AcroFields.Fields.Select(item => item.Key);
            foreach (var cellName in listFieldNames)
                Console.WriteLine(string.Format("FIELD: {0}", cellName));

            return listFieldNames;
        }

        /// <summary>
        /// Fill PDF Cells/Fields 
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="fileName">File name</param>
        /// <param name="listFieldNames">List with field names</param>
        private void FillForm(string filePath, string fileName, IEnumerable<string> listFieldNames)
        {
            var fullPathFile = $@"{filePath}\{fileName}.pdf";
            if (!FileExists(filePath, fileName)) return;

            string newFile = $@"{filePath}\filled-{fileName}.pdf";
            PdfReader pdfReader = new PdfReader(fullPathFile);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(
                newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;

            foreach (var field in listFieldNames)
                pdfFormFields.SetField(field, "OI");

            pdfStamper.FormFlattening = false;
            // close the pdf

            pdfStamper.Close();
        }

        /// <summary>
        /// Check if files exists
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        private bool FileExists(string filePath, string fileName)
        {
            var fullPathFile = $@"{filePath}\{fileName}.pdf";
            if (!Directory.Exists(filePath) || !File.Exists(fullPathFile))
            {
                Console.WriteLine("Cannot find file path ou pdf file on pc.");
                return false;
            }

            return true;
        }
    }
}
