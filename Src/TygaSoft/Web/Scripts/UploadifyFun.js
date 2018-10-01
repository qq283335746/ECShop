
function OnUploadify(h) {
    var obj = document.getElementById(h);
    $(obj).uploadify({
        'swf': '/Jquery/plugins/uploadify/scripts/uploadify.swf',
        'uploader': '/ScriptServices/UploadService.asmx/UploadifyUpload',
        'width': 80,
        'height':26,
        'auto': false,
        'multi': true,
        'buttonText': '选择文件',
        'fileTypeDesc': '支持的文件格式：',
        'fileTypeExts': '*.gif; *.jpg; *.png',
        'onUploadSuccess': function (file, data, response) {
            OnUploadSuccess(data);
        },
        'onQueueComplete': function (queueData) {
            OnQueueComplete();
        }
    });
}