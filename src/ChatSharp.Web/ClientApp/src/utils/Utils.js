export const equal = (firstItem, secondItem) => {
    if (isNullOrEmpty(firstItem) && isNullOrEmpty(secondItem)) {
        return true;
    }

    if (!isNullOrEmpty(firstItem) && isNullOrEmpty(secondItem)) {
        return false;
    }

    if (isNullOrEmpty(firstItem) && !isNullOrEmpty(secondItem)) {
        return false;
    }

    const firstItemToString = firstItem.toString().toLowerCase();
    const secondItemToString = secondItem.toString().toLowerCase();

    return firstItemToString === secondItemToString;
}

export const isNullOrEmpty = (data) => {
    if (data === undefined || data === '' || data === null || data === 'undefined' || data === 'null') {
        return true;
    }

    var stringData = data.toString();

    try {
        stringData = stringData.trim();
    } catch (e) {
        return true;
    }

    return (!stringData || 0 === stringData.length);
}

export function showAndHide(targetToShow, targetToHide, duration) {
    let ctlToShow = document.querySelector(targetToShow);
    let ctlToHide = document.querySelector(targetToHide);

    var duration = duration ?? 200;

    // Initially hide the control to show and show the control to hide
    ctlToShow.style.display = 'none';
    ctlToHide.style.display = 'block';

    // Add the transition classes
    ctlToHide.classList.add('hide');
    ctlToShow.classList.remove('hide');
    ctlToShow.classList.add('show');

    setTimeout(function () {
        // After the duration has passed, swap the display properties
        ctlToHide.style.display = 'none';
        ctlToShow.style.display = 'block';
    }, duration);
}

export function generateUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (Math.random() * 16) | 0,
            v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
    });
}

export function formatMessageForHTML(message) {
    if (isNullOrEmpty(message)) {
        return '';
    }

    message = message.replace(/\n/g, '<br />');
    while (message.startsWith('<br>') || message.startsWith('<br />')) {
        message = message.replace(/^(<br>|<br \/>)/, '');
    }

    while (message.endsWith('<br>') || message.endsWith('<br />')) {
        message = message.replace(/(<br>|<br \/>$)/, '');
    }

    return message;
};

export function isMatchGuid(match) {
    if (match && match.params && match.params.guid) {
        const guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;
        return guidRegex.test(match.params.guid);
    }

    return false;
}