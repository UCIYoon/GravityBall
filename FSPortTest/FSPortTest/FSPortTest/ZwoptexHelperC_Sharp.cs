//
//  ZwoptexHelperC_Sharp.cs
//  FSPortTest
//  http://zwoptexapp.com/flashversion
//  Zwoptex app is sprite sheet generator for Apple iOS system.
//  Let's hack into C Sharp project!
//
//  Created by Yoon Lee on 12/29/10.
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
        XmlTextReader reader = null;

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
            Dictionary<string, object> mainCollection = null;

            while (reader.Read())
            {
                Dictionary<string, object> subCollection = null;

                // first we meet the frames tag
                if (reader.Value.Equals("frames"))
                {
                    mainCollection = new Dictionary<string, object>();
                }
                else if (mainCollection != null)
                {
                    // then we need to predict the dictionaries under frames dictionary
                    //if (reader.Name.Equals("dict"))
                        //subCollection = new Dictionary<string, object>();
                    //else if (subCollection != null)
                    Console.WriteLine(reader.Name);
                }
            }

            reader.Close();
        }
    }
}
