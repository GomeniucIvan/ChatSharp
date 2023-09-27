const prefixRoute = "/api/";

export const get = async (sufixRoute) => {
    const url = `${prefixRoute}${sufixRoute}`;

    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': localStorage.getItem('access_token'),
            'RefreshToken': localStorage.getItem('refresh_token'),
            'AccessGuid': localStorage.getItem('access_guid'),
        },
        credentials: 'include'
    });

    const jsonData = await response.json();
    return jsonData;
};

export const post = async (sufixRoute, object) => {
    const url = `${prefixRoute}${sufixRoute}`;

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': localStorage.getItem('access_token'),
            'RefreshToken': localStorage.getItem('refresh_token'),
            'AccessGuid': localStorage.getItem('access_guid'),
        },
        credentials: 'include',
        body: JSON.stringify(object)
    });

    const jsonData = await response.json();
    return jsonData;
};