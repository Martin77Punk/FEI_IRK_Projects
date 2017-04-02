using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FEI.IRK.HM.RMR.Lib
{
    public static class RobotSensorHelper
    {

        /// <summary>
        /// Checks validity of the iRobot Create Sensor Data file
        /// </summary>
        /// <param name="RobotSensorFile">Full path to file with iRobot Create Sensor data</param>
        /// <returns>TRUE if file is in specified format and does not contain errors, otherwise FALSE</returns>
        public static Boolean CheckFile(String RobotSensorFile)
        {
            Boolean FileCheck;
            String FileErrors;
            ProcessFile(RobotSensorFile, false, out FileCheck, out FileErrors);
            return FileCheck;
        }

        /// <summary>
        /// Checks validity of the iRobot Create Sensor Data file and return processing errors if any
        /// </summary>
        /// <param name="RobotSensorFile">Full path to file with iRobot Create Sensor data</param>
        /// <param name="ProcessingErrors">Returns Text with processing errors</param>
        /// <returns>TRUE if file is in specified format and does not contain errors, otherwise FALSE</returns>
        public static Boolean CheckFile(String RobotSensorFile, out String ProcessingErrors)
        {
            Boolean FileCheck;
            ProcessFile(RobotSensorFile, false, out FileCheck, out ProcessingErrors);
            return FileCheck;
        }


        /// <summary>
        /// Deserialize iRobot Create Sensor Data file
        /// </summary>
        /// <param name="RobotSensorFile">Full path to file with iRobot Create Sensor data</param>
        /// <returns>Deserialized iRobot Create Sensor Data from file</returns>
        public static RobotSensorDataList Deserialize(String RobotSensorFile)
        {
            Boolean FileCheck;
            String FileErrors;
            RobotSensorDataList SensorsData = ProcessFile(RobotSensorFile, true, out FileCheck, out FileErrors);
            return SensorsData;
        }

        /// <summary>
        /// Deserialize iRobot Create Sensor Data file and return processing errors if any
        /// </summary>
        /// <param name="RobotSensorFile">Full path to file with iRobot Create Sensor data</param>
        /// <param name="ProcessingErrors">Returns Text with processing errors</param>
        /// <returns>Deserialized iRobot Create Sensor Data from file</returns>
        public static RobotSensorDataList Deserialize(String RobotSensorFile, out String ProcessingErrors)
        {
            Boolean FileCheck;
            RobotSensorDataList SensorsData = ProcessFile(RobotSensorFile, true, out FileCheck, out ProcessingErrors);
            return SensorsData;
        }


        /// <summary>
        /// Will process specified file with iRobot Create Sensor Data
        /// </summary>
        /// <param name="RobotSensorFile">Full path to file with iRobot Create Sensor data</param>
        /// <param name="DoDeserialize">If enabled file will be deserialized into return value object. If disabled it will only check file</param>
        /// <param name="ProcessingSuccess">Returns TRUE if file was sucesfully processed. Returns FALSE otherwise</param>
        /// <param name="ProcessingErrors">Returns Text with processing errors</param>
        /// <returns>Deserialized iRobot Create Sensor Data from file (only in case DoDeserialize is TRUE, otherwise NULL)</returns>
        private static RobotSensorDataList ProcessFile(String RobotSensorFile, Boolean DoDeserialize, out Boolean ProcessingSuccess, out String ProcessingErrors)
        {
            long FileSize = 0;
            int BytesRead = 0;
            byte[] FileData;
            int DataCount = 0;
            int i = 0;
            int lastTimeStamp = 0;
            RobotSensorDataList SensorDataList = null;
            RobotSensorData PreviousData = null;

            int IRobotStructureSize = Marshal.SizeOf(typeof(RobotSensorDataStruct));

            // Check file exists
            if (!File.Exists(RobotSensorFile))
            {
                ProcessingSuccess = false;
                ProcessingErrors = String.Format("Súbor '{0}' neexistuje!", Path.GetFileName(RobotSensorFile));
                return null;
            }

            // Get file size
            try
            {
                FileSize = new FileInfo(RobotSensorFile).Length;
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
                using (FileStream RobotSensorFileStream = File.OpenRead(RobotSensorFile))
                {
                    BytesRead = RobotSensorFileStream.Read(FileData, 0, FileData.Length);
                    if ((long)BytesRead != FileSize)
                    {
                        ProcessingSuccess = false;
                        ProcessingErrors = String.Format("Chyba pri čítaní obsahu súboru '{0}'!", Path.GetFileName(RobotSensorFile));
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

            // Check if measurements data from file fits in structure + get count of structures
            if ((FileSize % IRobotStructureSize) != 0)
            {
                ProcessingSuccess = false;
                ProcessingErrors = String.Format("Súbor '{0}' nie je platný súbor senzorových dát iRobot Create!", Path.GetFileName(RobotSensorFile));
                return null;
            }
            DataCount = (int)(FileSize / IRobotStructureSize);

            // Create empty list for deserialization
            if (DoDeserialize)
            {
                SensorDataList = new RobotSensorDataList();
            }

            // Read data structures
            try
            {                
                for (i = 0; i < DataCount; i++)
                {
                    // Copy specific data to new byte array
                    byte[] SingleSensorReading = new byte[IRobotStructureSize];
                    Array.Copy(FileData, (i * IRobotStructureSize), SingleSensorReading, 0, IRobotStructureSize);

                    // read and transform byte array to structure
                    GCHandle handle = GCHandle.Alloc(SingleSensorReading, GCHandleType.Pinned);
                    RobotSensorDataStruct sensorReadings = (RobotSensorDataStruct) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(RobotSensorDataStruct));
                    handle.Free();

                    // Check whether accept or ignore reading based on invalid timestamp
                    if (sensorReadings.Timestamp < lastTimeStamp)
                        continue;
                    else
                        lastTimeStamp = sensorReadings.Timestamp;

                    // Add sensor readings to the list
                    if (DoDeserialize)
                    {
                        RobotSensorData NewReadings = new RobotSensorData(sensorReadings, PreviousData);
                        SensorDataList.Add(NewReadings);
                        PreviousData = NewReadings;
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
            if (DoDeserialize)
            {
                ProcessingErrors = String.Format("Súbor senzorových dát iRobot Create '{0}' úspešne nahraný!", Path.GetFileName(RobotSensorFile));
            }
            else
            {
                ProcessingErrors = String.Format("Súbor '{0}' je platný súbor senzorových dát robota iRobot Create!", Path.GetFileName(RobotSensorFile));
            }
            
            return SensorDataList;

        }


    }
}
