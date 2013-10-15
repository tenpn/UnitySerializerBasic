UnitySerializerBasic
====================

A stripped-back version of UnitySerializer (http://whydoidoit.com/unityserializer/) for just serializing objects.

Use when you want to take an arbitary object, and convert it to JSON, a string or binary blob. At a later poitn the object can be instantiated again. 

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
