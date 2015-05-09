ko.bindingHandlers.dateTimePicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
       
        $(element).on("click", function (e) {
            var bindingData = ko.utils.unwrapObservable(valueAccessor());
            var onChangeDateHandler = bindingData.onDateChange;
            var value = bindingData.value || ko.observable();
            var language = 'en';
            var format = 'DD-MMM-YYYY';

            options = {
                defaultDate: value(),
                language: language,
                format: format,
                pickTime: false
            };

            $(element).datetimepicker(options);
            $(element).datetimepicker().show();
            $(element).on("dp.change", function (e) {
                var jsDate = new Date(e.date.format(appConfig.dateTimeFormat.dateTimeUTCFull));
                value(jsDate);
                if (onChangeDateHandler) {
                    onChangeDateHandler(jsDate);
                }
            });
            $(element).data("DateTimePicker").show();
        });
    }
};

function SessionModel(data) {
    var self = this;
    self.Id = ko.observable(data.Id) || ko.observable();
    self.StartTime = ko.observable(data.StartTime) || ko.observable();
    self.EndTime = ko.observable(data.EndTime) || ko.observable();
    self.Room = ko.observable(data.Room) || ko.observable();
    self.Weekday = ko.observable(data.Weekday) || ko.observable();
    self.TypeName = "ClassSession";
    self.inEditMode = ko.observable(data.inEditMode) || ko.observable(false);
    self.hasError = ko.observable(false);
    return self;
}

function CourseViewModel(data) {
    var self = this;
    self.Id = data.Id;
    self.Name = data.Name ? ko.observable(data.Name) : ko.observable();
    self.ProfessorName = data.ProfessorName ? ko.observable(data.ProfessorName) : ko.observable();
    self.StartDate = data.StartDate ? ko.observable(parseDate(data.StartDate)) : ko.observable();
    self.EndDate = data.EndDate ? ko.observable(parseDate(data.EndDate)) : ko.observable();
    self.ExcusedTime = data.ExcusedTime ? ko.observable(parseTime(data.ExcusedTime)) : ko.observable();
    self.TypeName = "Class";
    self.Sessions = ko.observableArray([]);
    self.editModeEnabled = ko.observable(false);
    self.isNewSession = ko.observable(false);
    self.isError = ko.observable(false);
    self.isSuccess = ko.observable(false);
    self.resultMessage = ko.observable();
    self.formatDate = function (date) {
        if (date instanceof Date) {
            return moment(date).format('DD-MMM-YYYY');
        } else {
            return date === undefined ? "" : moment(date).format('DD-MMM-YYYY');
        }
    };
    self.deleteClass = function(sender, e) {
        /*Call performDeleteClass() then show the result.*/
        performDeleteClass();
        jumpToMessage();
    };
    self.saveClass = function () {
        /*Refine data then call performSaveClass() then show the result.*/
        performSaveClass();        
        jumpToMessage()
    };
    self.addNewSession = function () {
        if (self.editModeEnabled()) {
            alert("Please save your current work before continue.");
        } else {
            var session = new SessionModel({
                inEditMode: true
            });
            self.Sessions.push(session);
            self.editModeEnabled(true);
            self.isNewSession(true);
        }
    };
    self.enableEditMode = function (sender,e) {
        if (self.editModeEnabled()) {
            alert("Please save your current work before continue.");
        } else {
            sender.inEditMode(true);
            self.editModeEnabled(true);
        }
    };
    self.saveSession = function (sender, e) {
        if (isValidTimeFormat(sender.StartTime()) && isValidTimeFormat(sender.EndTime())) {
            sender.inEditMode(false);
            sender.hasError(false);
            self.editModeEnabled(false);
            self.isNewSession(false);
        } else {
            alert("Invalid time fortmat. The input data must be in [hh:mm] fortmat, eg. '09:30'.");
            sender.hasError(true);
            return;
    }
       
    };
    self.deleteSession = function (sender, e) {
        if (self.editModeEnabled()) {
            alert("Please save your current work before continue.");
        } else {
            if (confirm('You\'re to delete this session?')) {
                self.Sessions.remove(sender);
            }
            return;
        }
    };
    self.cancelChange = function (sender, e) {
        self.editModeEnabled(false);
        self.isNewSession(false);
        self.Sessions.remove(sender);
    };
    

    /*init class session data*/
    addSession();

    //private function to initialize session data
    function addSession() {
        for (i = 0; i < data.Sessions.length; i++) {
            data.Sessions[i].StartTime = parseTime(data.Sessions[i].StartTime);            
            data.Sessions[i].EndTime = parseTime(data.Sessions[i].EndTime);
            data.Sessions[i].Weekday = parseWeekday(data.Sessions[i].Weekday);
            self.Sessions.push(new SessionModel(data.Sessions[i]));
        }
    }

    function isValidTimeFormat (value) {
        var regx = /(^[0-2][0-9]:[0-5][0-9]$){1}/;
        if (regx.test(value)) {
            var t = value.split(":");
            return (Number(t[0]) <= 23);
        }
        return false;
    }

    function parseDate(value)
    {
        value = value.slice(6, value.length - 2);
        var d = Number(value);
        var date = new Date(d);
        return date;
    }

    function parseTime(value) {
        var h = value.Hours > 9 ? value.Hours : '0' + value.Hours;
        var m = value.Minutes > 9 ? value.Minutes : '0' + value.Minutes;
        var str = h + ':' + m;        
        return str;
    }

    function parseWeekday(value) {
        switch (value) {
            case 1:
                return "Mon";
            case 2:
                return "Tue";
            case 3:
                return "Wed";
            case 4:
                return "Thu";
            case 5:
                return "Fri";
            case 6:
                return "Sat";
            default:
                return "Sun";
        }
    }

    /*to be continued */

    function performDeleteClass() {
        /*Send request to server to delete this class and return the result.*/
        var uri = composeUrl("/aasadmin/deletecourse");
        $.ajax({
            url: uri,
            method: 'DELETE',
            data: {Id: self.Id}
        }).done(function () {
            self.isSuccess(true);
            self.resultMessage("The " + self.Name() + " course by " + self.ProfessorName() + " has been deleted.");
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.isError(true);
            self.resultMessage("Status: " + jqXHR.status + ". Error: " + errorThrown);
        });
    }

    function performSaveClass() {
        /*Send request to server to save this class and show the result.*/
        var uri = composeUrl("/aasadmin/course");
        $.ajax({
            url: uri,
            method: 'POST',
            data: {
                course: refineData(),
                startDate: moment(self.StartDate()).format('DD-MMM-YYYY'),
                endDate: moment(self.EndDate()).format('DD-MMM-YYYY')
            }
        }).done(function () {
            self.isSuccess(true);
            self.resultMessage("The " + self.Name() + " course by " + self.ProfessorName() + " has been saved.");
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.isError(true);
            self.resultMessage("Status: " + jqXHR.status + ". Error: " + errorThrown);
        });
    }

    function refineData() {
        /*refine data before sending to server*/
        var sessionlist = [];
        for(i = 0; i < self.Sessions().length; i++){
            sessionlist.push({
                Id : self.Sessions()[i].Id(),
                StartTime: self.Sessions()[i].StartTime(),
                EndTime : self.Sessions()[i].EndTime(),
                Room : self.Sessions()[i].Room(),
                Weekday : self.Sessions()[i].Weekday(),
                TypeName : "ClassSession",
            });
        }
        return {
            Id : self.Id,
            Name : self.Name(),
            ProfessorName : self.ProfessorName(),
            StartDate : self.StartDate(),
            EndDate: self.EndDate(),
            ExcusedTime : self.ExcusedTime(),
            TypeName : "Class",
            Sessions : sessionlist
        };
    }

    function jumpToMessage() {
        var msg = document.getElementById('result-message').offsetTop;
        window.scrollTo(0, top);
    }

    function composeUrl(relativeUrl) {
        if (!relativeUrl) return window.aasweb;
        return window.aasweb.replace(/\/+$/, '') + "/" + relativeUrl.replace(/^\/+/, '');
    }
}