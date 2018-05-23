$(document).ready(function () {

    var currentOwner = GetCurrentOwner();

    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: "agendaWeek",
        weekends: false,
        minTime: "07:00:00",
        maxTime: "20:00:00",
        height: "auto",
        navLinks: false, // can click day/week names to navigate views
        selectable: IsAuthorized(currentOwner),
        selectHelper: true,

        // called when the user clicks and drags on the calendar
        select: function (start, end) {
            ClearPopupFormValues(); // clear the popup window
            // populate event form with what we can
            $('#eventID').val(-1);
            $('#dateBox').val(moment(start).format("MM/DD/YYYY"));
            $('#startTime').val(moment(start).format("HH:mm:ss"));
            $('#endTime').val(moment(end).format("HH:mm:ss"));
            // show the event form and focus on title
            $('#popupEventForm').show();
            $('#eventTitle').focus();
        },

        editable: false,
        eventStartEditable: false,
        eventLimit: true, // allow "more" link when too many events

        // makes a call to the get events endpoint
        events: '/Dashboard/GetEvents?Owner=' + currentOwner + '&Room=' + GetCurrentOwnerRoom(),

        // called when an event is clicked on
        eventClick: function (calEvent, jsEvent, view) {
            if (IsAuthorized(currentOwner)) {
                ClearPopupFormValues(); // clear the popup window
                // populate event form
                $('#eventID').val(parseInt(calEvent.id));
                $('#eventTitle').val(calEvent.title);
                $('#dateBox').val(moment(calEvent.start).format("MM/DD/YYYY"));
                $('#startTime').val(moment(calEvent.start).format("HH:mm:ss"));
                $('#endTime').val(moment(calEvent.end).format("HH:mm:ss"));
                // show the event form and focus on title
                $('#popupEventForm').show();
                $('#eventTitle').focus();
            }
        }
    });

    // function to update a calendar event entry
    function UpdateEvent(EventID, Title, EventStart, EventEnd, RoomNumber, EventOwner, EventColor) {
        var dataRow = {
            'ID': EventID,
            'Title': Title,
            'Start': EventStart,
            'End': EventEnd,
            'Room': RoomNumber,
            'Owner': EventOwner,
            'Color': EventColor
        };
        $.ajax({
            type: 'POST',
            url: "/Dashboard/InsertCalendarEvent",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(dataRow),
            success: function (response) {
                if (response.success === true) {
                    $('#calendar').fullCalendar('refetchEvents');
                }
                else {
                    alert('Error! Could not save the event!');
                }
            },
            error: function (response) {
                alert('Error! Could not save the event!');
            }
        });
    }

    // function to do a get request
    function httpGet(theUrl) {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", theUrl, false); // false for synchronous request
        xmlHttp.send(null);
        return xmlHttp.responseText;
    }

    // shows the create event dialog
    function ShowEventPopup(date) {
        ClearPopupFormValues();
        $('#popupEventForm').show();
        $('#eventTitle').focus();
    }

    // clears the popup form of values
    function ClearPopupFormValues() {
        $('#eventID').val(-1);
        $('#eventTitle').val("");
        $('#eventDateTime').val("");
        $('#eventDuration').val("");
    }

    // function for when the cancel button is clicked on the popup form
    $('#btnPopupCancel').click(function () {
        ClearPopupFormValues();
        $('#popupEventForm').hide();
    });

    // called when the save button is clicked on the popup form
    $('#btnPopupSave').click(function () {
        $('#popupEventForm').hide(); // hide the popup form

        // build the start and end date
        // check wether id is set
        var id_t = $('#eventID').val();
        if (parseInt(id_t) === -1) {
            // no previous id existed, get the next one
            id_t = httpGet("/Dashboard/GetNextId");
        }
        var title_t = $('#eventTitle').val();
        var date_t = moment($('#dateBox').val()).format("YYYY-MM-DD");
        var start_t = $('#startTime').val();
        var end_t = $('#endTime').val();

        var owner = currentOwner;
        var room = GetCurrentOwnerRoom();
        var eventColor = "";
        if (document.getElementById('cancelCheckbox').checked) {
            eventColor = "#ad3a3a";
        } else {
            eventColor = "#337ab7";
        }

        ClearPopupFormValues();
        UpdateEvent(id_t, title_t, date_t + "T" + start_t, date_t + "T" + end_t, room, owner, eventColor);
    });

    // runs when the delete button is clicked
    $('#btnPopupDelete').click(function () {
        // hide form
        $('#popupEventForm').hide();

        // get the id
        var id_t = $('#eventID').val();

        var dataRow = {
            'ID': id_t
        };

        $.ajax({
            type: 'POST',
            url: "/Dashboard/DeleteCalendarEvent",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(dataRow),
            success: function (response) {
                if (response.success === true) {
                    $('#calendar').fullCalendar('refetchEvents');
                }
                else {
                    alert('Error! Could not delete the event!');
                }
            },
            error: function (response) {
                alert('Error response! Could not delete the event!');
            }
        });
    });

    // function to return the current user
    function GetCurrentOwner() {
        // get the owner
        var owner = "";
        $.ajax({
            async: false,
            type: 'GET',
            url: '/Dashboard/GetCurrentUser',
            success: function (data) {
                owner = data;
            }
        });

        return owner;
    }

    // function to return the room number for current user
    function GetCurrentOwnerRoom() {
        // get the room
        var room = "";
        $.ajax({
            async: false,
            type: 'GET',
            url: '/Dashboard/GetCurrentRoom',
            success: function (data) {
                room = data;
            }
        });

        return room;
    }

    // function to return whether user is autherized for editing events
    function IsAuthorized(currentUser) {
        var extension = currentUser.substr(currentUser.indexOf('@') + 1, currentUser.length);
        if (extension === "sdsmt.edu") {
            return true;
        }
        return false;
    }
});