import React, { createContext, useContext, useState, useCallback } from 'react';
import network from '../network.json';

const GlobalContext = createContext();

export const GlobalProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem("token") || null);

  // 토큰 검증
  const validateToken = (cb) => {
    if (!token) {
      cb({ valid: false });
      return;
    }
    GetApiAsync('Validate', {}, cb);
  }

  const GetAdminServerUrl = useCallback((apiUrl) => {
    return `https://${network.Host}:${network.Port}/InHouse/${apiUrl}`;
  }, []);

  const InternalSendApiAsync = useCallback(async (apiUrl, meth, request, cb) => {
    try {
      const url = GetAdminServerUrl(apiUrl);

      const options = {
        method: meth.toUpperCase(),
        headers: { 'Content-Type': 'application/json',
          ...(token ? {'Authorization': `Bearer ${token}`} : {}) //JWT 추가
         }
      };

      if (options.method !== 'GET') {
        options.body = JSON.stringify(request || {});
      }

      const data = await fetch(url, options).then(res => res.json());
      cb(data);
    } catch (err) {
      alert(`SendAPIError | ${err}`);
    }
  }, [token]);

  const PostApiAsync = (apiUrl, request, cb) => {
    return InternalSendApiAsync(apiUrl, 'post', request, cb);
  };

  const GetApiAsync = (apiUrl, request, cb) => {
    return InternalSendApiAsync(apiUrl, 'get', request, cb);
  };

  return (
    <GlobalContext.Provider value={{
      token,
      setToken,
      validateToken,
      GetAdminServerUrl,
      PostApiAsync,
      GetApiAsync
    }}>
      {children}
    </GlobalContext.Provider>
  );
};

export const useGlobalStore = () => useContext(GlobalContext);
