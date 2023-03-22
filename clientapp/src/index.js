import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

ReactDOM.createRoot(document.getElementById('root')).render(
    //禁用严格模式，因为开发环境下 useEffect 会调用两次，打包后不会调用两次
    //<React.StrictMode>
    <App />
    //</React.StrictMode>
);
