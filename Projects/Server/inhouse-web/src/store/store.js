// store.js
import React, { createContext, useContext, useState, useCallback } from 'react';
import network from '../network.json';

export const SceneEnum = {
  Login: 0,
  Home: 1
};

const GlobalContext = createContext();

export const GlobalProvider = ({ children }) => {
  const [scene, setScene] = useState(SceneEnum.Login);

  const GetAdminServerUrl = useCallback((apiUrl) => {
    return `https://${network.Host}:${network.Port}/InHouse/${apiUrl}`;
  }, []);

  const InternalSendApiAsync = useCallback(async (apiUrl, meth, request, cb) => {
    try {
      const url = `https://${network.Host}:${network.Port}/InHouse/${apiUrl}`;
      console.log("url : ", url);
      const data = await fetch(url, {
        method: meth,
        body: JSON.stringify({ request }),
        headers: {
          'Content-Type': 'application/json'
        }
      }).then(res => res.json());
      cb(data);
    } catch (err) {
      alert(`SendAPIError | ${err}`);
    }
  }, []);

  const PostApiAsync = (apiUrl, request, cb) => {
    return InternalSendApiAsync(apiUrl, 'post', request, cb);
  };

  const GetApiAsync = (apiUrl, request, cb) => {
    return InternalSendApiAsync(apiUrl, 'get', request, cb);
  };

  return (
    <GlobalContext.Provider value={{
      scene, setScene,
      GetAdminServerUrl,
      PostApiAsync,
      GetApiAsync
    }}>
      {children}
    </GlobalContext.Provider>
  );
};

export const useGlobalStore = () => useContext(GlobalContext);
