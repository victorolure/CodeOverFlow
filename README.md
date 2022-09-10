# SD-330-W22SD-Assignment
This web-application project is a blog modelled after stackoverflow/reddit.

The main Language is C# with SQLServer as database.

Auhorized users can have access to various functionality based on their "ROLES".

Users can ask questions, answer questions, comment on questions, comment on answers, react to questions and answers(upvote & downvote)

These functionalities are enabled by various async Tasks present in asigned Controllers.

An authorized user can ask or answer a question  but cannot react to the question, Only other authorized users can add an upvote or downvote to the question or answer.

Only the user that asked a question has the option to mark an answer as correct.

Reactions to answers increases or decreases the reputation of the user that asked or answered the question.

A user with the Admin role is the only user authorized to delete questions completely from the database. Deletion of questions completely deletes all the associated comments and answers.

Users without authorization can view all questions on the blog but cannot react until they're logged in.

Some seed data are also added to the database for test.
