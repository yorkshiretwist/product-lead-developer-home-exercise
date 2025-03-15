# UK Parliament - Product Team Home Exercise for Lead Developer

Thank you for the opportunity to participate in this exercise. I can't say I've enjoyed it very much! Hopefully it's not _complete_ poppycock.

## Excuses, excuses

The back-end API I found fairly easy, as that's something I've done a lot of recently. However I found the front-end development challenging, as it's been several years since I did any Angular development - and even then I was only adding features to an existing app. Front-end frameworks are _not_ my chosen specialist subject

## Things I would like to fix/change

There are many things I would like to fix or change in this implementation. A non-exhuastive list:

- Implement pagination for the search results table. The server-side code has been prepared, but the client-side component has not been developed.
- Handling validation errors. Validation errors are returned from the server, but they are not displayed in the UI. I had limited time and so decided to try to submit an app that mostly hung together rather than one which didn't work at all but where validation errors displayed perfectly.
- When a person's name is first clicked the URI changes however the person form is not displayed. Everything works from the second click onwards. I'm sure this is due to my lack of understanding about Angular routing.
- When creating a new person if you click 'Clear' it displays the form fields as if they are invalid. This is wrong.
- I would like to incorporate more styling from parliament.uk, and make the app more responsive. Alas, time is the enemy of us all.
- It irks me that the `PersonViewModel` has a separate `departmentId` and `departmentName` properties. It would be much nicer to have a `department` property which itself has `id` and `name` properties, but weird things happend in Angular when I did that. This, again, is my lack of understanding about Angular at fault.

## There's always another way

I did wonder if it would have been acceptable to develop this simple app _without_ Angular, in fact without any SPA framework at all. I know that will sound heretical, please let me explain!

An app as simple as this requires little in the way of interactivity. Loading 3.84MB of JavaScript, as is the case here, feels ... a lot. A similar level of interactivity could be achieved by writing a small amount of 'vanilla' JavaScript to fetch rendered HTML from the server, rather than transferring JSON which needs additional parsing and binding to the DOM using a very large library.

This 'throw chunks of HTML around' approach has been taken by some recent frameworks (Astro being one example: https://astro.build/). Not only does this lead to vastly lighter front-ends, but means the app is server-rendered by default, and all templating is done using server-side technologies. The ethos behind this is progressive enhancement - of which you can probably tell I'm a big fan.

Anyway, if I were to do this exercise again I would probably ask if it's acceptable to create the app using _any_ front-end technique of choice. However, for now, this is my submission.

Thank you for reading my rant :)

## Update

I have developed a reponse to this exercise using an alternative approach, please see details in this repo: https://github.com/yorkshiretwist/product-lead-developer-home-exercise-custom