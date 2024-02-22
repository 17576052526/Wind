import alert from './alert';


export default function () {
  return (
      <>
          <input type="button" value="测试弹出框" onClick={() => alert('测试一下111')} />
      </>
  );
}
