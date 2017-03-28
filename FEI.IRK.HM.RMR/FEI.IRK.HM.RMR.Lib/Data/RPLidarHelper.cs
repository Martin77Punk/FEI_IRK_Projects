using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public static class RPLidarHelper
    {

        /// <summary>
        /// Checks validity of the RPLidar Scan Data file
        /// </summary>
        /// <param name="RPLidarMeasurementFile">Full path to file with RPLidar Scan data</param>
        /// <returns>TRUE if file is in specified format and does not contain errors, otherwise FALSE</returns>
        public static Boolean CheckFile(String RPLidarMeasurementFile)
        {
            Boolean FileCheck;
            String FileErrors;
            ProcessFile(RPLidarMeasurementFile, false, out FileCheck, out FileErrors);
            return FileCheck;
        }

        /// <summary>
        /// Checks validity of the RPLidar Scan Data file and return processing errors if any
        /// </summary>
        /// <param name="RPLidarMeasurementFile">Full path to file with RPLidar Scan data</param>
        /// <param name="ProcessingErrors">Returns Text with processing errors</param>
        /// <returns>TRUE if file is in specified format and does not contain errors, otherwise FALSE</returns>
        public static Boolean CheckFile(String RPLidarMeasurementFile, out String ProcessingErrors)
        {
            Boolean FileCheck;
            ProcessFile(RPLidarMeasurementFile, false, out FileCheck, out ProcessingErrors);
            return FileCheck;
        }


        /// <summary>
        /// Deserialize RPLidar Scan Data file
        /// </summary>
        /// <param name="RPLidarMeasurementFile">Full path to file with RPLidar Scan data</param>
        /// <returns>Deserialized RPLidar Scan Data from file</returns>
        public static RPLidarMeasurementList Deserialize(String RPLidarMeasurementFile)
        {
            Boolean FileCheck;
            String FileErrors;
            RPLidarMeasurementList RPLidarMeasurementData = ProcessFile(RPLidarMeasurementFile, true, out FileCheck, out FileErrors);
            return RPLidarMeasurementData;
        }

        /// <summary>
        /// Deserialize RPLidar Scan Data file and return processing errors if any
        /// </summary>
        /// <param name="RPLidarMeasurementFile">Full path to file with RPLidar Scan data</param>
        /// <param name="ProcessingErrors">Returns Text with processing errors</param>
        /// <returns>Deserialized RPLidar Scan Data from file</returns>
        public static RPLidarMeasurementList Deserialize(String RPLidarMeasurementFile, out String ProcessingErrors)
        {
            Boolean FileCheck;
            RPLidarMeasurementList RPLidarMeasurementData = ProcessFile(RPLidarMeasurementFile, true, out FileCheck, out ProcessingErrors);
            return RPLidarMeasurementData;
        }


        /// <summary>
        /// Will process specified file with RPLidar Scan data
        /// </summary>
        /// <param name="RPLidarMeasurementFile">Full path to file with RPLidar Scan data</param>
        /// <param name="DoDeserialize">If enabled file will be deserialized into return value object. If disabled it will only check file</param>
        /// <param name="ProcessingSuccess">Returns TRUE if file was sucesfully processed. Returns FALSE otherwise</param>
        /// <param name="ProcessingErrors">Returns Text with processing errors</param>
        /// <returns>Deserialized RPLidar Scan Data from file (only in case DoDeserialize is TRUE, otherwise NULL)</returns>
        private static RPLidarMeasurementList ProcessFile(String RPLidarMeasurementFile, Boolean DoDeserialize, out Boolean ProcessingSuccess, out String ProcessingErrors)
        {

            long FileSize = 0;
            int BytesRead = 0;
            byte[] FileData;
            string[] FileLines;
            int i = 0, j = 0;
            int MeasurementsCount = 0;
            RPLidarMeasurementList RPLidarMeasurementData = null;
            RPLidarMeasurement PreviousMeasurement = null;

            // Check file exists
            if (!File.Exists(RPLidarMeasurementFile))
            {
                ProcessingSuccess = false;
                ProcessingErrors = String.Format("Súbor '{0}' neexistuje!", Path.GetFileName(RPLidarMeasurementFile));
                return null;
            }

            // Get file size
            try
            {
                FileSize = new FileInfo(RPLidarMeasurementFile).Length;
            }
            catch (Exception e)
            {
                ProcessingSuccess = false;
                ProcessingErrors = e.Message;
                return null;
            }

            // Prepare bytes
            FileData = new byte[FileSize];

            // Read file contents
            try
            {
                using (FileStream RPLidarFileStream = File.OpenRead(RPLidarMeasurementFile))
                {
                    BytesRead = RPLidarFileStream.Read(FileData, 0, FileData.Length);
                    if ((long)BytesRead != FileSize)
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Chyba pri čítaní obsahu súboru '{0}'!", Path.GetFileName(RPLidarMeasurementFile));
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ProcessingSuccess = false;
                ProcessingErrors = e.Message;
                return null;
            }

            // Check file bytes
            // Possible file bytes:
            //  - Numbers (ASCII 18-57 / 0x30 - 0x39)
            //  - Dot (ASCII 16 / 0x2E)
            //  - Space (ASCII 32 / 0x20)
            //  - Tab (ASCII 9 / 0x9)
            //  - Linux newline \n (ASCII 10 / 0xA)
            foreach (byte FileByte in FileData)
            {
                if (!Enumerable.Range(0x30, 0x39).Contains(FileByte) && !Enumerable.Range(0x9, 0xA).Contains(FileByte) && FileByte != 0x2E && FileByte != 0x20)
                {
                    ProcessingSuccess = false;
                    ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Neplatné znaky", Path.GetFileName(RPLidarMeasurementFile));
                    return null;
                }
            }

            try
            {
                // Read file lines as text
                FileLines = File.ReadAllLines(RPLidarMeasurementFile);

                // Lines count must be a multipler of 3
                if (FileLines.Length == 0 || (FileLines.Length % 3) != 0)
                {
                    ProcessingSuccess = false;
                    ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Nesprávny počet riadkov", Path.GetFileName(RPLidarMeasurementFile));
                    return null;
                }

                // Prepare Measurements Count
                MeasurementsCount = (int)(FileLines.Length / 3);

                // Initialize List
                if(DoDeserialize)
                {
                    RPLidarMeasurementData = new RPLidarMeasurementList();
                }

                // Separators prepare - Space + Tab
                char[] RPLidSeparators = new char[2];
                RPLidSeparators[0] = ' ';
                RPLidSeparators[1] = '\t';


                // ForEach Measurement
                for (i = 0; i < MeasurementsCount; i++)
                {
                    int MeasurementCount = 0;
                    int MeasurementTimestamp = 0;
                    double[] MeasurementDistances;
                    double[] MeasurementAngles;

                    // Read ScanCount and Timestamp
                    String[] MeasurementOps = FileLines[(i * 3)].Split(RPLidSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (MeasurementOps.Length != 2)
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Nesprávne zadaný počet meraní/timestamp\nRiadok: {1}", Path.GetFileName(RPLidarMeasurementFile), (i * 3));
                        return null;
                    }
                    if(!Int32.TryParse(MeasurementOps[0], out MeasurementCount))
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Nemôžem prečítať počet meraní\nRiadok: {1}", Path.GetFileName(RPLidarMeasurementFile), (i * 3));
                        return null;
                    }
                    if (!Int32.TryParse(MeasurementOps[1], out MeasurementTimestamp))
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Nemôžem prečítať timestamp\nRiadok: {1}", Path.GetFileName(RPLidarMeasurementFile), (i * 3));
                        return null;
                    }

                    // Read Distance list
                    String[] MeasurementDistancesStr = FileLines[(i * 3) + 1].Split(RPLidSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if(MeasurementDistancesStr.Length != MeasurementCount)
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Počet vzdialeností nesedí s počtom meraní\nRiadok: {1}", Path.GetFileName(RPLidarMeasurementFile), (i * 3) +1);
                        return null;
                    }
                    MeasurementDistances = new double[MeasurementCount];
                    for (j = 0; j < MeasurementCount; j++)
                    {
                        double SingleDistance;
                        if(!Double.TryParse(MeasurementDistancesStr[j], NumberStyles.Number, CultureInfo.InvariantCulture, out SingleDistance))
                        {
                            ProcessingSuccess = false;
                            ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Nemôžem správne prečítať vzdialenosť\nRiadok: {1}, Meranie: {2}", Path.GetFileName(RPLidarMeasurementFile), (i * 3) + 1, j + 1);
                            return null;
                        }
                        MeasurementDistances[j] = SingleDistance;
                    }

                    // Read Angles
                    String[] MeasurementAnglesStr = FileLines[(i * 3) + 2].Split(RPLidSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if(MeasurementAnglesStr.Length != MeasurementCount)
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Počet uhlov nesedí s počtom meraní\nRiadok: {1}", Path.GetFileName(RPLidarMeasurementFile), (i * 3) + 2);
                        return null;
                    }
                    MeasurementAngles = new double[MeasurementCount];
                    for (j = 0; j < MeasurementCount; j++)
                    {
                        double SingleAngle;
                        if (!Double.TryParse(MeasurementAnglesStr[j], NumberStyles.Number, CultureInfo.InvariantCulture, out SingleAngle))
                        {
                            ProcessingSuccess = false;
                            ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor RPLidar meraní. \nChyba: Nemôžem správne prečítať uhol\nRiadok: {1}, Meranie: {2}", Path.GetFileName(RPLidarMeasurementFile), (i * 3) + 2, j + 1);
                            return null;
                        }
                        MeasurementAngles[j] = SingleAngle;
                    }

                    // Scans for single Measurement read - save it
                    if (DoDeserialize)
                    {
                        RPLidarScanList ScanList = new RPLidarScanList();
                        for (j = 0; j < MeasurementCount; j++)
                        {
                            RPLidarScan NewScan = new RPLidarScan(MeasurementDistances[j], MeasurementAngles[j]);
                            ScanList.Add(NewScan);
                        }
                        RPLidarMeasurement NewMeasurement = new RPLidarMeasurement(MeasurementTimestamp, ScanList, PreviousMeasurement);
                        RPLidarMeasurementData.Add(NewMeasurement);
                        PreviousMeasurement = NewMeasurement;
                    }

                }

            }
            catch (Exception e)
            {
                ProcessingSuccess = false;
                ProcessingErrors = e.Message;
                return null;
            }

            // OK
            ProcessingSuccess = true;
            ProcessingErrors = String.Format("RPLidar súbor '{0}' úspešne nahraný!", Path.GetFileName(RPLidarMeasurementFile));
            return RPLidarMeasurementData;
        }




    }
}
