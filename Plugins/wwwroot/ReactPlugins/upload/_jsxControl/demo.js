import UploadImg from './upload-img'
import UploadFile from './upload-file'

export default function () {
    return (
        <>
            <UploadImg multiple={true} data="/upload/20240223/1708667097282946.jpg|/upload/20240223/1708667097804681.png" getData={(data) => console.log(data)} isFormal={true}></UploadImg>
            <UploadFile multiple={true} data="/upload/20240223/1708667097282946.jpg|/upload/20240223/1708667097804681.png" getData={(data) => console.log(data)} isFormal={true}></UploadFile>
        </>
    );
}
