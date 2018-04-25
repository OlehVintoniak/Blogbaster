var SpinnerHelper = {};
SpinnerHelper.Spinner = new Spinner();
SpinnerHelper.isSpinnerSpin = false;
SpinnerHelper.showSpinner = function () {
    if (!SpinnerHelper.isSpinnerSpin) {
        SpinnerHelper.isSpinnerSpin = true;
        SpinnerHelper.Spinner.spin(SpinnerHelper.spinnerContainer);
    }
};
SpinnerHelper.hideSpinner = function () {
    SpinnerHelper.Spinner.stop();
    setTimeout(function () {
        SpinnerHelper.isSpinnerSpin = false;
    }, 100);
};
SpinnerHelper.spinnerContainer = document.getElementById("spinner-container");