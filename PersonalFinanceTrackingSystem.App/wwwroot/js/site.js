window.showSweetAlert = (type, message) => {
    if (type === "success") {
        Swal.fire({
            icon: "success",
            title: "Success!",
            text: message,
            showConfirmButton: false,
            timer: 2000 // Auto-close after 2 seconds
        });
    } else if (type === "error") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: message,
        });
    }
};