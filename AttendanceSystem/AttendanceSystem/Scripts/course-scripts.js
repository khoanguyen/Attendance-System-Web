function editSession(id) {
    alert("To be implemented...");
}

function saveSession() {

}

function cancelSessionChange() {
    
}

function enableDisableEditMode(id, isEnable){
    var editModeClassName = 'edit-mode-enable';
    var editModelClassSelect = '.edit-mode-enable';
    var spanSelector = '#' + id + ' > td > span';
    var inputSelector = '#' + id + ' > td input';
    var btnEditSelector = '#' + id + ' > td button[name="btnEdit"]';
    var btnSaveSelector = '#' + id + ' > td button[name="btnSave"]';
    var btnDeleteSelector = '#' + id + ' > td button[name="btnDelete"]';
    var btnCancelSelector = '#' + id + ' > td button[name="btnCancel"]';

    if (isEnable) {
        $('#' + id).addClass(editModeClassName);
        $(spanSelector).addClass('hidden');
        $(inputSelector).removeClass('hidden');
        $(btnEditSelector).addClass('hidden');
        $(btnSaveSelector).removeClass('hidden');
        $(btnDeleteSelector).addClass('hidden');
        $(btnCancelSelector).removeClass('hidden');
    } else {
        $(spanSelector).removeClass('hidden');
        $(inputSelector).addClass('hidden');
        $(btnEditSelector).removeClass('hidden');
        $(btnSaveSelector).addClass('hidden');
        $(btnDeleteSelector).removeClass('hidden');
        $(btnCancelSelector).addClass('hidden');
        $('#' + id).removeClass(editModeClassName);
    }
}

function deleteSession(id) {
    alert("To be implemented...");
}

function deleteClass(classId) {
    if (confirm("Do you really want to delete this course?")) {
        alert("To be implemented...");
    }
}