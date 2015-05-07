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
    return self;
}

function CourseViewModel(data) {
    var self = this;
    self.Id = ko.observable(data.Id);
    self.Name = ko.observable(data.Name);
    self.ProfessorName = ko.observable(data.ProfessorName);
    self.StartDate = ko.observable(parseDate(data.StartDate));
    self.EndDate = ko.observable(parseDate(data.EndDate));
    self.ExcusedTime = ko.observable(parseTime(data.ExcusedTime));
    self.TypeName = "Class";
    self.Sessions = ko.observableArray([]);
    self.editModeEnabled = ko.observable(false);
    self.isNewSession = ko.observable(false);
    self.formatDate = function (date) {
        if (date instanceof Date) {
            return moment(date).format('DD-MMM-YYYY');
        } else {
            return date === undefined ? "" : moment(date).format('DD-MMM-YYYY');
        }
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
            self.editModeEnabled(false);
            self.isNewSession(false);
        } else {
            alert("Invalid time fortmat. The input data must be in [hh:mm] fortmat, eg. '09:30'.");
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

    function addSession() {
        for (i = 0; i < data.Sessions.length; i++) {
            data.Sessions[i].StartTime = parseTime(data.Sessions[i].StartTime);            
            data.Sessions[i].EndTime = parseTime(data.Sessions[i].EndTime);
            data.Sessions[i].Weekday = parseWeekday(data.Sessions[i].Weekday);
            self.Sessions.push(new SessionModel(data.Sessions[i]));
        }
    }

    function isValidTimeFormat (value) {
        var regx = /([0-2][0-9]:[0-5][0-9]){1}/;
        if (regx.test(value)) {
            var t = value.split(":");
            return (Number(t[0]) <= 23);
        }
        return regx.test(value);
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

    function deleteClass() {
        /*Send request to server to delete this class and show the result.*/
    }

    function saveClass() {
        /*Send request to server to save this class and show the result.*/
    }

    function deleteClass() {
        /*Send request to server to delete this class and show the result.*/
    }

    function refineData() {
        /*refine data before sending to server*/
    }

    function parseStringToTime(value) {
       /*convert string of time to time object*/
    }
}