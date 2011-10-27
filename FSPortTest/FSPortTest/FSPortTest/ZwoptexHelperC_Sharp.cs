//
//  ZwoptexHelperC_Sharp.cs
//  FSPortTest
//  http://zwoptexapp.com/flashversion
//  Zwoptex app is sprite sheet generator for Apple iOS system.
//  Let's hack into C Sharp project!
//
//  Created by Yoon Lee on 10/26/11.
//  Copyright 2010 University of California, Irvine. All rights reserved.
//
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace FSPortTest
{
    class ZwoptexHelperC_Sharp
    {
        private XmlTextReader reader = null;
        public Dictionary<String, Dictionary<String, String>> frameDictionary = null;

        // parameter
        public ZwoptexHelperC_Sharp(String filename)
        {
            if (!File.Exists(filename))
                throw new System.ArgumentNullException(".cannot find the file");

            // load the .plist
            reader = new XmlTextReader(filename);
        }

        // structure
        // <key>texture</key> <-- we don't need
        // <key>frames</key> <-- we need
        // patterns
        // <key>imagename</key>
        // <dict> x, y, offsetsXY, height+width
        public void parseStart()
        {
            while (reader.Read())
            {
                Dictionary<String, String> imageOrientation = null;
                String imageNameKey = "";

                // first we meet the frames tag
                if (reader.Value.Equals("frames"))
                {
                    frameDictionary = new Dictionary<String, Dictionary<String, String>>();
                    reader.Read();
                    reader.Read();
                    reader.Read();
                    reader.Read();
                    // same act as scan.next() that we want to skip the <dict>
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.Equals("dict"))
                                {
                                    imageOrientation = new Dictionary<String, String>();
                                    String imageElementKey = null;
                                    Boolean _QUIT = false;

                                    // another loop for sub tree
                                    while (!_QUIT && reader.Read())
                                    {
                                        switch (reader.NodeType)
                                        {
                                            case XmlNodeType.Element:
                                                if (reader.Name.Equals("key"))
                                                    imageElementKey = this.getKeyTextureValue();
                                                break;
                                            case XmlNodeType.Text:
                                                imageOrientation.Add(imageElementKey, reader.Value);
                                                break;
                                            case XmlNodeType.EndElement:
                                                if (reader.Name.Equals("dict"))
                                                {
                                                    _QUIT = true;
                                                    frameDictionary.Add(imageNameKey, imageOrientation);
                                                    imageOrientation = null;
                                                    imageNameKey = null;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else if (reader.Name.Equals("key"))
                                {
                                    // STEP 2: PULL IMAGE NAME
                                    imageNameKey = this.getKeyTextureValue();
                                }
                                break;
                        }
                    }
                }
            }

            reader.Close();
        }

        private String getKeyTextureValue()
        {
            String strRet = null;

            while (reader.Read()&&strRet==null)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                        strRet = reader.Value;
                        break;
                }
            }

            return strRet;
        }

        public String toString()
        {
            String retContents = null;

            foreach (var images in frameDictionary)
            {
                Dictionary<String, String> image = frameDictionary[images.Key];
                retContents += "imageNamed: '" + images.Key + "' = {\n";
                foreach (var contents in image)
                {
                    retContents += "key:" + contents.Key + " value:" + contents.Value + "\n";
                }

                retContents += "}\n";
            }

            return retContents;
        }
    }
}
