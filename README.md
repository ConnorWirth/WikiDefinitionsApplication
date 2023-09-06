# WikiDefinitionsApplication

## Scenario
You have accepted the role of a Senior Programmer for CITE Managed Services, your task is to develop a fully functional Wiki application for the junior programmers. The initial Wiki prototype has been approved for full development, however, CITE management have requested some alterations to the original specifications.

## Background Information
There are many different categories and definitions for Data Structures used in software development; CITE management would like to see a uniform definition and cataloguing of this information. They have supplied some specific details but would like you to complete the task. 

## Introduction
Before the project can commence you need to review the Management Criteria and complete the Wiki Application Proposal, GUI Design, Class Details and Project Details for submission. Once your proposal has been authorised (signed off) by the CITE representative (Your Lecturer) you can begin the next stage of the Wiki development.

You should consult with the CITE representative (Your Lecturer) if you are unsure about any of the problems or questions. Your primary research should focus on the resources on the Blackboard website, additional information can be collected from the Internet, ensure all sources are referenced at the end of your report. You should write your answers in the standard templates provided in this assessment document. 

## Management Criteria
The Wiki application must use a List<T> of a simple class which implements an IComparable<T> interface. This single class must have the following attributes: Name, Category, Structure and Definition, (refer Data Structure Matrix at the end of this document). Following the success of the prototype, management would like the user to have the following functionality: user can add, edit and delete Data Structure information. During this process the system must be able to prevent duplicate Names and filter out numeric or special character input. The user can select a Data Structure name from the list of Names and the associated information will be displayed in the related text boxes.

The application must have a search feature so a user can find a specific Data Structure by entering the Name into the search textbox and clicking the search button, if the Name is found the associated information will be displayed in the related text boxes. The search textbox must clear when the search is completed and refocus the I beam cursor into the Name textbox. After a successful search the Data Structure Name in the list must be highlighted; a double mouse click in the Name input box which will clear all textboxes, this must have an associated tool tip.

The Wiki application will save data when the form closes. There are two buttons for the manual open and save option; this must use a dialog box to select a file or rename a saved file. All wiki data is stored/retrieved using a binary file format.

All user interactions must have full error trapping and feedback messaging which is displayed in a status strip at the bottom of the form. Use a message box for all critical errors with caption and icon.
