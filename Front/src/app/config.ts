export interface ConfigAttributes {
    BaseApi: string;
    HubApi: string;
    HasDebug: boolean;
}

let config: ConfigAttributes = {
    BaseApi: 'http://localhost:5130/api/',
    HubApi: 'http://localhost:5130/gamehub',

    // BaseApi: 'http://116.203.101.2/api/',
    // HubApi: 'http://116.203.101.2/gamehub',
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
