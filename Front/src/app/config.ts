export interface ConfigAttributes {
    BaseApi: string;
    HubApi: string;
    HasDebug: boolean;
}

// @ts-expect-error get custom debug config from html if needed
const debugConf = debugConfig;
let config: ConfigAttributes = debugConf || {
    BaseApi: 'http://localhost:5130/api/',
    HubApi: 'http://localhost:5130/gamehub',
    HasDebug: true,
};
if (process.env.NODE_ENV && process.env.NODE_ENV === 'production') {
    // production code
    config = {
        BaseApi: window.location.origin + '/api',
        HubApi: window.location.origin + '/gamehub',
        HasDebug: false,
    };
}

export default config;
