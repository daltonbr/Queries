using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YleJson
{
    // http://developer.yle.fi/api_docs.html

    [Serializable]
    /// <summary>
    /// Represents a collections of items (programs, episodes or clips) in the Yle WebAPI
    /// Used in the API Json. This is root level of the Json
    /// </summary>
    public class Items
    {
        public string apiVersion;
        public Datum[] data;
        public Meta meta;
    }
    /// <summary>
    /// Basic Metadata from the query    
    /// </summary>
    [Serializable]
    public class Meta
    {
        public int clip;
        public int count;
        public string limit;
        public string offset;
        public int program;
        public string q;
    }
    [Serializable]
    /// <summary>
    /// Represents an item (program, episode or clip) in the Yle WebAPI
    /// </summary>
    public class Datum
    {
        public string[] alternativeId;
        public Audio[] audio;
        public string collection;
        public Contentrating contentRating;
        public object[] countryOfOrigin;
        public object[] creator;
        public Description description;
        public string duration;
        public string id;
        public Image image;
        public DateTime indexDataModified;
        public Itemtitle itemTitle;
        public Originaltitle originalTitle;
        public Partofseries partOfSeries;
        public Publicationevent[] publicationEvent;
        public Subject1[] subject;
        public object[] subtitling;
        public Title4 title;
        public string type;
        public string typeCreative;
        public string typeMedia;
        public Video video;
    }
    [Serializable]
    public class Contentrating
    {
        public object[] reason;
        public Title title;
    }
    [Serializable]
    public class Title
    {
    }
    [Serializable]
    public class Description
    {
        public string fi;
    }
    [Serializable]
    public class Image
    {
        public bool available;
        public string id;
        public string type;
    }
    [Serializable]
    public class Itemtitle
    {
        public string fi;
    }
    [Serializable]
    public class Originaltitle
    {
    }
    [Serializable]
    public class Partofseries
    {
        public string @Id;
        public Availabilitydescription availabilityDescription;
        public object[] countryOfOrigin;
        public Coverimage coverImage;
        public object[] creator;
        public Description1 description;
        public string id;
        public Image1 image;
        public Interaction[] interactions;
        public Subject[] subject;
        public Title1 title;
        public string type;
    }
    [Serializable]
    public class Availabilitydescription
    {
        public string fi;
    }
    [Serializable]
    public class Coverimage
    {
        public bool available;
        public string id;
        public string type;
    }
    [Serializable]
    public class Description1
    {
        public string fi;
    }
    [Serializable]
    public class Image1
    {
        public bool available;
        public string id;
        public string type;
    }
    [Serializable]
    public class Title1
    {
        public string fi;
    }
    [Serializable]
    public class Interaction
    {
        public Title2 title;
        public string type;
        public string url;
    }
    [Serializable]
    public class Title2
    {
        public string fi;
    }
    [Serializable]
    public class Subject
    {
        public Broader broader;
        public string id;
        public string inScheme;
        public string key;
        public Title3 title;
        public string type;
    }
    [Serializable]
    public class Broader
    {
        public string id;
    }
    [Serializable]
    public class Title3
    {
        public string fi;
        public string sv;
    }
    [Serializable]
    public class Title4
    {
        public string fi;
    }
    [Serializable]
    public class Video
    {
    }
    [Serializable]
    public class Audio
    {
        public Format[] format;
        public string[] language;
        public string type;
    }
    [Serializable]
    public class Format
    {
        public string inScheme;
        public string key;
        public string type;
    }
    [Serializable]
    public class Publicationevent
    {
        public string duration;
        public DateTime endTime;
        public string id;
        public Media media;
        public string region;
        public Service service;
        public DateTime startTime;
        public string temporalStatus;
        public string type;
        public Publisher[] publisher;
    }
    [Serializable]
    public class Media
    {
        public bool available;
        public Contentprotection[] contentProtection;
        public bool downloadable;
        public string duration;
        public string id;
        public string type;
    }
    [Serializable]
    public class Contentprotection
    {
        public string id;
        public string type;
    }
    [Serializable]
    public class Service
    {
        public string id;
    }
    [Serializable]
    public class Publisher
    {
        public string id;
    }
    [Serializable]
    public class Subject1
    {
        public Broader1 broader;
        public string id;
        public string inScheme;
        public string key;
        public Title5 title;
        public string type;
        public Notation[] notation;
    }
    [Serializable]
    public class Broader1
    {
        public string id;
    }
    [Serializable]
    public class Title5
    {
        public string fi;
        public string sv;
        public string en;
    }
    [Serializable]
    public class Notation
    {
        public string value;
        public string valueType;
    }
}