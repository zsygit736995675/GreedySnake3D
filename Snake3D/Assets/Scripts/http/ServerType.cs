using UnityEngine;

public enum ServerType {
    [InspectorName("晓飞本地测试服务器:192.168.2.1:9005")]
    LOCAL,

    [InspectorName("测试服:test.tools.beijingqianji.com")]
    DEBUG,

    [InspectorName("正式服:cn0.tools.beijingqianji.com")]
    RELEASE,


}