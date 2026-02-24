// Dataverse Form Logic
// Handles visibility of the "general_approval" section based on the statuscode field value.

var FormLogic = FormLogic || {};

FormLogic.StatusCode = {
    Pending: 545630001
};

/**
 * Called on form OnLoad and on statuscode field OnChange.
 * Shows or hides the "general_approval" section based on whether
 * the statuscode field is set to Pending (545630001).
 * @param {Xrm.Events.EventContext} executionContext - The execution context passed from the form event.
 */
FormLogic.onStatusCodeChange = function (executionContext) {
    var formContext = executionContext.getFormContext();
    var statusCodeAttribute = formContext.getAttribute("statuscode");

    if (!statusCodeAttribute) {
        return;
    }

    var statusCodeValue = statusCodeAttribute.getValue();
    var isPending = statusCodeValue === FormLogic.StatusCode.Pending;

    formContext.ui.tabs.forEach(function (tab) {
        tab.sections.forEach(function (section) {
            if (section.getName() === "general_approval") {
                section.setVisible(isPending);
            }
        });
    });
};
