using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.IO;

namespace PetroOperaciones.Controllers.Facturacion.invoiceToPDF.tools
{
    public class FileManager
    {

        static readonly List<string> _extensions = new List<string> { "*.pdf" };

        private ArrayList getFilesOfInputDirectory(String path)
        {
            ArrayList filePaths = new ArrayList();
            String inputFolder = path;

            try
            {
                foreach (String ext in _extensions)
                {
                    foreach (string subFile in Directory.GetFiles(inputFolder, ext))
                    {
                        if (!filePaths.Contains(subFile))
                        {
                            filePaths.Add(subFile);
                        }

                    }

                }


            }
            catch (Exception fe)
            {
                //log.Error(fe.Message);
            }


            return filePaths;
        }


        public void deletePDFFiles(String path)
        {


            ArrayList filePaths = getFilesOfInputDirectory(path);

            foreach (String file in filePaths)
            {
                deleteSourceFile(file);
            }


        }

        private void deleteSourceFile(String file)
        {
            try
            {
                File.Delete(file);
            }
            catch (IOException fe)
            {
                //log.Error(fe.Message);
            }

        }
    }
}