window.successMessage = (message) => {
    console.log("Success");
    Notiflix.Report.success(
        'Success!',
        message,
        'Ok',
    );
}

window.errorMessage = (message) => {

    Notiflix.Report.failure(
        'Error!',
        message,
        'Ok',
    );
}

window.ConfirmMessageBox = (message) => {
    return new Promise((resolve) => {
        Notiflix.Confirm.show(
            'Confirm',
            message,
            'Yes',
            'No',
            function okCb() {
                resolve(true); // Resolve with true if "Yes" is clicked
            },
            function cancelCb() {
                resolve(false); // Resolve with false if "No" is clicked
            }
        );
    });
};
