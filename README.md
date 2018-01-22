# Queries

This is a code assignment in Unity3D 👨🏻‍💻  
The objective is to work with some concepts:

* **JSON** files (in Unity3d) 🤷🏻‍
* **WebRequests** (from a webAPI)
* Dinamic **scrollable lists** (in Unity UI)

![Final Animation](./screenshots/queriesDemo.gig)  
*Application flow*

## Youtube Teaser
<a href="http://www.youtube.com/watch?feature=player_embedded&v=HRKVRhoazTc
" target="_blank"><img src="http://img.youtube.com/vi/HRKVRhoazTc/hqdefault.jpg"
width="240" height="180" border="10" /></a>

## The assignment

The task is to search programs from the [Yle API](http://developer.yle.fi/tutorials.html)

* The application should have an input field where the user can provide what programs to search for (**a search query**).
* The **Finnish title** of each result should be displayed in a scrollable list.
* When submitting a search, only the **first 10 results** should be retrieved from the API.
* When the **user scrolls** to the last few items in the list, the next 10 results should be **appended** to the list.
* Pressing on the row should display more details about the program. For this you can select any 5 revelant fields from the JSON.

## Technologies to use

* Unity UI
* UnityWebRequest
* Any JSON library you prefer

## Results

* I find that the **Unity Json Utility** has some limitations (e.g. we can't easily import an array of objects)
* I found (and use here) a port [Json.Net.Unity3D](https://github.com/SaladLab/Json.Net.Unity3D) of the famous [Newtonsoft Json.NET](www.newtonsoft.com/json) library
* To build to iOS I have to tweak some things in XCode (more information in the Port github above)
* Essentialy it's a good practice to use **Coroutines** when doing webRequest so we have **non-blocking requests**
* Visual Studio has awesome tools to "rebuild" a Json Structure into an OOP-ish object
