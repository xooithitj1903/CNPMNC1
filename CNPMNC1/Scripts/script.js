function Changelmage(Up10adImage, previewlmg) {
    if (Uploadlmage.files && Uploadlmage.files[ø]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewlmg).attr(' src ',
                e.target.result);
            reader.readAsDataURL(Uploadlmage.files[0]);
        }
    }
}