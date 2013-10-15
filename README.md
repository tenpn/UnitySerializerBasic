UnitySerializerBasic 1.0
========================

A stripped-back version of UnitySerializer (http://whydoidoit.com/unityserializer/) for just serializing objects.

Use when you want to take an arbitary object, and convert it to JSON, a string or binary blob. At a later point the object can be instantiated again. 

Installation
============

Download latest package from https://github.com/tenpn/UnitySerializerBasic/releases  
From Unity, Assets->Import Package->Custom Package...

Usage
=====

The serializer will grab all public properties and members of a class or struct. If you have private members and properties that need serializing, mark them with the [SerializeThis] property. Alternativly, mark public members and properties that don't need serializing with [DoNotSerialize]. 

Then to convert an object to a string containing binary data, call:  

    var stringRepresentation = Storage.SerializeToString(myObj);

To convert back:  

    var recreatedObject = Storage.Deserialize<ObjectType>(stringRepresentation);

There are also functions to write directly to files, with JSON or binary blobs. See UnitySerializer.cs for more information.

Justification
=============

I wanted to persist a few classes between save games. These are not MonoBehaviours, so I assumed it would be quite simple to serialize them.

There were several options available. I could use .Net's XmlSerializer to get something working, but it's a little slow, the results are bulky, and it can't persist classes with private fields without you extending the classes. 

.Net also has a BinaryFormatter which can be used to quickly make small representations of classes, and it works with (annotated) private fields. However depending on your Unity project settings, this can cause JIT issues on iOS.

The main UnitySerializer package (http://whydoidoit.com/unityserializer/) can do JIT-aware binary serialization on mobile, but is a heavyweight package designed to help you create a whole savegame system. So I took that package (very generously available under the MIT licence) and pulled out the core serialization code, leaving something that's lighter, works on mobile, and is easy to use in client code.

Contributions
=============

Pull requests for further streamlining or bugfixes are welcome. For anything else, check first that I'll accept your modifications. 
