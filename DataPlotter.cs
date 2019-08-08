using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap {
    public class DataPlotter : MonoBehaviour
    {
        //Name of the input file (eg. staticPlane)
        public string inputfile;

        //Holds data from CSV file
        private List<Dictionary<string, object>> pointList;

        //Point that is instantiated in the for loop
        private GameObject dataPoint;

        //Count of selected points
        private int count = 0;

        //Otherwise, graph plots everytime image is tracked
        private bool lmao = false;

        //For scaling (Didn't use)
        public float plotScale = 10;

        //Using it as string for selecting point
        public int intPointName = 0;

        //Prefab for instantiated points
        public GameObject PointPrefab;

        //Holds instantiated points
        public GameObject PointHolder;

        //Column index for excel file
        public int column0 = 0;
        public int column1 = 1;
        public int column2 = 2;
        public int column3 = 3;
        public int column4 = 4;
        public int column5 = 5;
        public int column6 = 6;
        public int column7 = 7;

        //Column names
        public string Latitude;
        public string Longitude;
        public string Altitude;
        public string Track;
        public string Model;
        public string Departure;
        public string Arrival;
        public string FlightName;

        //Text on headcanvas
        public Text _lat = null;
        public Text _long = null;
        public Text _track = null;
        public Text _alt = null;
        public Text _model = null;
        public Text _dept = null;
        public Text _arr = null;
        public Text _name = null;

        //Array holding values of selected points' details
        private string[] lat = new string[6000];
        private string[] longx = new string[6000];
        private string[] track = new string[6000];
        private string[] alt = new string[6000];
        private string[] model = new string[6000];
        private string[] dept = new string[6000];
        private string[] arr = new string[6000];
        private string[] namex = new string[6000];

        //Changes color of selected point
        public void changeColor ()
        {
            if(intPointName == (count))
            {
                intPointName = 0;
            }
            else
            {
                intPointName++;
            }

            
            for(int i=0; i<(count); i++)
            {
                Transform namePoint = PointHolder.gameObject.transform.GetChild(i);
              
                if(namePoint.name == System.Convert.ToString(intPointName))
                {
                    namePoint.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    namePoint.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            planeData();
            
            
        }

        void OnEnable()
        {
            if(lmao == false)
            {
                Debug.Log("Enable");
                pointList = CSVReader.Read(inputfile); //Reads CSV into pointList

                List <string> columnList = new List <string>(pointList[1].Keys); //Fill with column names

                foreach(string key in columnList)
                    Debug.Log("Column name is " + key);

                //Column names assign
                Latitude = columnList[column0];
                Longitude = columnList[column1];
                Altitude = columnList[column3];
                Track = columnList[column2];
                Model = columnList[column4];
                Departure= columnList[column5];
                Arrival = columnList[column6];
                FlightName = columnList[column7];

                //Max
                float xMax = FindMaxValue(Latitude);
                float yMax = FindMaxValue(Longitude);
                float zMax = FindMaxValue(Altitude);

                //Min
                float xMin = FindMinValue(Latitude);
                float yMin = FindMinValue(Longitude);
                float zMin = FindMinValue(Altitude);
               
             
                //Loop through Pointlist
                for (var i = 0; i < pointList.Count; i++)
                {
                    //Fit into Scotland's map
                    if ((55.5 <= (System.Convert.ToSingle(pointList[i][Latitude]))) && ((System.Convert.ToSingle(pointList[i][Latitude])) <= 70)
                    && (-8 <= (System.Convert.ToSingle(pointList[i][Longitude]))) && ((System.Convert.ToSingle(pointList[i][Longitude])) <= -1))
                    {
                        count++;
                        float x = (System.Convert.ToSingle(pointList[i][Latitude]) - xMin) / (xMax - xMin);
                        x = x * (float)1.8;

                        float y = (System.Convert.ToSingle(pointList[i][Longitude]) - yMin) / (yMax - yMin);

                        y = y * (float)2.5;

                        float z = (System.Convert.ToSingle(pointList[i][Altitude]) - zMin) / (zMax - zMin);
                        z = z * (float)1;

                       
                        //Quaternion newQuat = new Quaternion();
                        //newQuat.Set(0, 90, 90, 1);
                        dataPoint = Instantiate(PointPrefab, new Vector3(x, z, -y), Quaternion.identity);
                        
                        dataPoint.transform.parent = PointHolder.transform; // Make child of PointHolder object, to keep points within container in hiearchy
                        

                        string dataPointName = pointList[i][Latitude] + " " + pointList[i][Longitude] + " " + pointList[i][Altitude];
                        Debug.Log(dataPointName);
                        dataPoint.transform.name = System.Convert.ToString(count);
                        Debug.Log("Count: "+count);
                      
                        lat[count] = "" + pointList[i][Latitude];
                        longx[count] = "" + pointList[i][Longitude];
                        alt[count] = "" + pointList[i][Altitude];
                        track[count] = "" + pointList[i][Track];
                        model[count] = "" + pointList[i][Model];
                        dept[count] = "" + pointList[i][Departure];
                        arr[count] = "" + pointList[i][Arrival];
                        namex[count] = "" + pointList[i][FlightName];

                    }

                 
                }

             
                for(int i=0; i<(count); i++)
                {
                    Debug.Log("Child: " + PointHolder.gameObject.transform.GetChild(i) + " Lat: " + System.Convert.ToString(pointList[i][Latitude]));
                }

                changeColor();
            }


            lmao = true;
        }


        private float FindMaxValue(string columnName)
        {
            //set initial value to first value
            float maxValue = Convert.ToSingle(pointList[0][columnName]);

            //Loop through Dictionary, overwrite existing maxValue if new value is larger
            for (var i = 0; i < pointList.Count; i++)
            {
                if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                    maxValue = Convert.ToSingle(pointList[i][columnName]);
            }

            //Spit out the max value
            return maxValue;
        }

        private float FindMinValue(string columnName)
        {

            float minValue = Convert.ToSingle(pointList[0][columnName]);

            //Loop through Dictionary, overwrite existing minValue if new value is smaller
            for (var i = 0; i < pointList.Count; i++)
            {
                if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                    minValue = Convert.ToSingle(pointList[i][columnName]);
            }

            return minValue;
        }
        
        //Prints data of selected point
        private void planeData()
        {
            for (int i = 0; i < (count); i++)
            {
                Transform namePoint = PointHolder.gameObject.transform.GetChild(i);
                // Debug.Log(namePoint);
                if (namePoint.name == System.Convert.ToString(intPointName))
                {
                   
                    _lat.text = "Latitude: " + lat[i+1];
                    _long.text = "Longitude: " + longx[i+1];
                    _alt.text = "Altitude: " + alt[i+1];
                    _model.text = "Airplane Model: " + model[i+1];
                    _track.text = "Track: " + track[i+1];
                    _dept.text = "Departure Airport: " + dept[i+1];
                    _arr.text = "Arrival Airport: " + arr[i+1];
                    _name.text = "Flight: " + namex[i+1];

                }
                
            }
        }
    }
}