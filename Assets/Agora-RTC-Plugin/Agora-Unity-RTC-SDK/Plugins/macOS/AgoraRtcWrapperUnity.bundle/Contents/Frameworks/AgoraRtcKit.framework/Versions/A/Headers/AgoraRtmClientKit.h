//
//  AgoraRtmClientKit.h
//  AgoraRtcKit
//
//  Copyright (c) 2022 Agora. All rights reserved.
//
#import <Foundation/Foundation.h>
/**
 The  `AgoraRtmClientKit` class is the entry point of the Agora RTM2 SDK.
 */
@class AgoraRtmClientKit;
@class AgoraRtmMessageEvent;
@class AgoraRtmTopicOption;
@class AgoraRtmJoinTopicOption;
@class AgoraRtmJoinChannelOption;
@class AgoraRtmClientConfig;
@class AgoraRtmStreamChannel;
@class AgoraRtmTopicInfo;
/**
 The error codes of rtm client.
 */
typedef NS_ENUM(NSInteger, AgoraRtmClientErrorCode) {
  /**
   * 10001: The topic already joined
   */
  AgoraRtmClientErrTopicAlreadyExist = 10001,
  /**
   * 10002: Exceed topic limiation when try to join new topic
   */
  AgoraRtmClientErrExceedCreateTopicLimitation = 10002,
  /**
   * 10003: Topic name is invalid
   */
  AgoraRtmClientErrInvalidTopicName = 10003,
  /**
   * 10004: Publish topic message failed
   */
  AgoraRtmClientErrPublishTopicFailed = 10004,
  /**
   * 10005: Exceed topic limitation when try to subscribe new topic
   */
  AgoraRtmClientErrExceedSubscribeTopicLimitation = 10005,
  /**
   * 10006: Exceed user limitation when try to subscribe new topic
   */
  AgoraRtmClientErrExceedUserLimitation = 10006,
  /**
   * 10007: Exceed channel limitation when try to join new channel
   */
  AgoraRtmClientErrExceedChannelLimitation = 10007,
  /**
   * 10008: The channel already joined
   */
  AgoraRtmClientErrAlreadyJoinChannel = 10008,
  /**
   * 10009: Try to perform channel related operation before joining channel
   */
  AgoraRtmClientErrNotJoinChannel = 10009,
};

/**
 Connection states between rtm sdk and agora server.
 */
typedef NS_ENUM(NSInteger, AgoraRtmClientConnectionState) {
  /**
   * 1: The SDK is disconnected with server.
   */
  AgoraRtmClientConnectionStateDisconnected = 1,
  /**
   * 2: The SDK is connecting to the server.
   */
  AgoraRtmClientConnectionStateConnecting = 2,
  /**
   * 3: The SDK is connected to the server and has joined a channel. You can now publish or subscribe to
   * a track in the channel.
   */
  AgoraRtmClientConnectionStateConnected = 3,
  /**
   * 4: The SDK keeps rejoining the channel after being disconnected from the channel, probably because of
   * network issues.
   */
  AgoraRtmClientConnectionStateReconnecting = 4,
  /**
   * 5: The SDK fails to connect to the server or join the channel.
   */
  AgoraRtmClientConnectionStateFailed = 5,
};

/**
 Reasons for connection state change.
 */

typedef NS_ENUM(NSInteger, AgoraRtmClientConnectionChangeReason) {
  /**
   * 0: The SDK is connecting to the server.
   */
  AgoraRtmClientConnectionChangedConnecting = 0,
  /**
   * 1: The SDK has joined the channel successfully.
   */
  AgoraRtmClientConnectionChangedJoinSuccess = 1,
  /**
   * 2: The connection between the SDK and the server is interrupted.
   */
  AgoraRtmClientConnectionChangedInterrupted = 2,
  /**
   * 3: The connection between the SDK and the server is banned by the server.
   */
  AgoraRtmClientConnectionChangedBannedByServer = 3,
  /**
   * 4: The SDK fails to join the channel for more than 20 minutes and stops reconnecting to the channel.
   */
  AgoraRtmClientConnectionChangedJoinFailed = 4,
  /**
   * 5: The SDK has left the channel.
   */
  AgoraRtmClientConnectionChangedLeaveChannel = 5,
  /**
   * 6: The connection fails because the App ID is not valid.
   */
  AgoraRtmClientConnectionChangedInvalidAppId = 6,
  /**
   * 7: The connection fails because the channel name is not valid.
   */
  AgoraRtmClientConnectionChangedInvalidChannelName = 7,
  /**
   * 8: The connection fails because the token is not valid.
   */
  AgoraRtmClientConnectionChangedInvalidToken = 8,
  /**
   * 9: The connection fails because the token has expired.
   */
  AgoraRtmClientConnectionChangedTokenExpired = 9,
  /**
   * 10: The connection is rejected by the server.
   */
  AgoraRtmClientConnectionChangedRejectedByServer = 10,
  /**
   * 11: The connection changes to reconnecting because the SDK has set a proxy server.
   */
  AgoraRtmClientConnectionChangedSettingProxyServer = 11,
  /**
   * 12: When the connection state changes because the app has renewed the token.
   */
  AgoraRtmClientConnectionChangedRenewToken = 12,
  /**
   * 13: The IP Address of the app has changed. A change in the network type or IP/Port changes the IP
   * address of the app.
   */
  AgoraRtmClientConnectionChangedClientIpAddressChanged = 13,
  /**
   * 14: A timeout occurs for the keep-alive of the connection between the SDK and the server.
   */
  AgoraRtmClientConnectionChangedKeepAliveTimeout = 14,
  /**
   * 15: The SDK has rejoined the channel successfully.
   */
  AgoraRtmClientConnectionChangedRejoinSuccess = 15,
  /**
   * 16: The connection between the SDK and the server is lost.
   */
  AgoraRtmClientConnectionChangedChangedLost = 16,
  /**
   * 17: The change of connection state is caused by echo test.
   */
  AgoraRtmClientConnectionChangedEchoTest = 17,
  /**
   * 18: The local IP Address is changed by user.
   */
  AgoraRtmClientConnectionChangedClientIpAddressChangedByUser = 18,
  /**
   * 19: The connection is failed due to join the same channel on another device with the same uid.
   */
  AgoraRtmClientConnectionChangedSameUidLogin = 19,
  /**
   * 20: The connection is failed due to too many broadcasters in the channel.
   */
  AgoraRtmClientConnectionChangedTooManyBroadcasters = 20,
};

/**
 rtm channel type.
 */
typedef NS_ENUM(NSInteger, AgoraRtmChannelType) {
  /**
   * 0: message channel.
   */
  AgoraRtmChannelTypeMessage = 0,
  /**
   * 1: stream channel.
   */
  AgoraRtmChannelTypeStream = 1,
};

/**
 * RTM presence type.
 */
typedef NS_ENUM(NSInteger, AgoraRtmPresenceType) {
  /**
   * 0: Triggered when remote user join channel
   */
  AgoraRtmPresenceTypeRemoteJoinChannel = 0,
  /**
   * 1: Triggered when remote leave join channel
   */
  AgoraRtmPresenceTypeRemoteLeaveChannel = 1,
  /**
   * 2: Triggered when remote user's connection timeout
   */
  AgoraRtmPresenceTypeRemoteConnectionTimeout = 2,
  /**
   * 3: Triggered when remote user join a topic
   */
  AgoraRtmPresenceTypeRemoteJoinTopic = 3,
  /**
   * 4: Triggered when remote user leave a topic
   */
  AgoraRtmPresenceTypeRemoteLeaveTopic = 4,
  /**
   * 5: Triggered when local user join channel
   */
  AgoraRtmPresenceTypeSelfJoinChannel = 5,
};

/**
 * RTM error code occurs in stream channel.
 */
typedef NS_ENUM(NSInteger, AgoraRtmStreamChannelErrorCode) {
  /**
   * 0: No error occurs.
   */
  AgoraRtmStreamChannelErrorOk = 0,
  /**
   * 1: Triggered when subscribe user exceed limitation
   */
  AgoraRtmStreamChannelErroExceedLimitation = 1,
  /**
   * 2: Triggered when unsubscribe inexistent user
   */
  AgoraRtmStreamChannelErrorUserNotExist = 2,
};

/**
 * The qos of rtm message.
 */
typedef NS_ENUM(NSInteger, AgoraRtmMessageQos) {
    /**
     * not ensure messages arrive in order.
     */
    AgoraRtmMessageQosUnordered = 0,
    /**
     * ensure messages arrive in order.
     */
    AgoraRtmMessageQosOrdered = 1,
};

/**
 * Create topic options.
 */
__attribute__((visibility("default"))) @interface AgoraRtmJoinTopicOption: NSObject
/**
   * The qos of rtm message.
   */
@property (nonatomic, assign) AgoraRtmMessageQos qos;

  /**
   * The metaData of topic.
   */
@property (nonatomic, nullable) NSData* meta;

@end

/**
 * Topic options.
 */
__attribute__((visibility("default"))) @interface AgoraRtmTopicOption: NSObject
/**
 * The list of users to subscribe.
 */
@property (nonatomic, copy, nullable) NSArray<NSString *> *users;
@end

/**
 * Join channel options.
 */
__attribute__((visibility("default"))) @interface AgoraRtmJoinChannelOption: NSObject
/**
* Token used to join channel.
*/
@property (nonatomic, copy, nullable) NSString *token;
@end

__attribute__((visibility("default"))) @interface AgoraRtmMessageEvent: NSObject
  /**
   * Which channel type, messageChannel or streamChannel
   */
@property (nonatomic, assign, readonly) AgoraRtmChannelType channelType;
  /**
   * The channel to which the message was published
   */
@property (nonatomic, copy, nonnull) NSString *channelName;
  /**
   * If the channelType is stChannel, which topic the message come from. only for stChannel type
   */
@property (nonatomic, copy, nonnull) NSString *channelTopic;
  /**
   * The payload
   */
@property (nonatomic, copy, nonnull) NSData *message;
  /**
   * The publisher
   */
@property (nonatomic, copy, nonnull) NSString *publisher;
@end

__attribute__((visibility("default"))) @interface AgoraRtmTopicInfo: NSObject
  /**
   * The name of the topic.
   */
@property (nonatomic, copy, nonnull) NSString *topic;

  /**
   * The number of publisher in current topic.
   */
@property (nonatomic, copy, nonnull) NSArray<NSString *> *publisherUserIds;

  /**
   * The metaData of publisher in current topic.
   */
@property (nonatomic, copy, nonnull) NSArray<NSData *> *publisherMetas;
@end


__attribute__((visibility("default"))) @interface AgoraRtmPresenceEvent: NSObject
/**
 * Which channel type, messageChannel or streamChannel
 */
@property (nonatomic, assign, readonly) AgoraRtmChannelType channelType;
/**
 * Can be remote join/leave channel, remote user join/leave topic or self join channel
 */
@property (nonatomic, assign, readonly) AgoraRtmPresenceType type;
/**
 * The channel to which the message was published
 */
@property (nonatomic, copy, nonnull) NSString *channelName;
/**
 * topic information array.
 */
@property (nonatomic, copy, nonnull) NSArray<AgoraRtmTopicInfo *> *topicInfos;
/**         uap
 * The user who triggered this event.
 */
@property (nonatomic, copy, nonnull) NSString *userId;
@end

/**
 *  Configurations for RTM Client.
 */
__attribute__((visibility("default"))) @interface AgoraRtmClientConfig: NSObject
/**
 * The App ID of your project.
 */
@property (nonatomic, copy, nonnull) NSString *appId;

/**
 * The ID of the user.
 */
@property (nonatomic, copy, nonnull) NSString *userId;
@end

@protocol AgoraRtmClientDelegate <NSObject>
@optional

/**
   * Occurs when receive a message.
   *
   * @param event details of message event.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onMessageEvent:(AgoraRtmMessageEvent * _Nonnull)event;

/**
   * Occurs when remote user join/leave channel, join/leave topic or local user joined channel.
   *
   * note:
   * When remote user join/leave channel will trigger this callback.
   * When remote user(in same channel) joinTopic/destroy Topic will trigger this callback.
   * When local user join channel will trigger this callback.
   *
   * For type(AgoraRtmPresenceTypeRemoteJoinChannel/AgoraRtmPresenceTypeRemoteLeaveChannel),
   * valid field will be channelType/type/channelName/userId
   * For type(AgoraRtmPresenceTypeRemoteJoinTopic/AgoraRtmPresenceTypeRemoteLeaveTopic)
   * valid field will be channelType/type/channelName/topicInfos/topicInfoNumber
   * For type(AgoraRtmPresenceTypeSelfJoinChannel)
   * valid field will be channelType/type/channelName/topicInfos/topicInfoNumber/userId
   * 
   * @param event details of presence event.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onPresenceEvent:(AgoraRtmPresenceEvent * _Nonnull)event;

/**
   * Occurs when user join a channel.
   *
   * @param channelName The Name of the channel.
   * @param userId The id of the user.
   * @param errorCode The error code.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onUser:(NSString * _Nonnull)userId
    joinChannel:(NSString * _Nonnull)channelName
    result:(AgoraRtmStreamChannelErrorCode)errorCode;

/**
   * Occurs when user leave a channel.
   *
   * @param channelName The Name of the channel.
   * @param userId The id of the user.
   * @param errorCode The error code.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onUser:(NSString * _Nonnull)userId
    leaveChannel:(NSString * _Nonnull)channelName
    result:(AgoraRtmStreamChannelErrorCode)errorCode;

/**
   * Occurs when user join topic.
   *
   * @param channelName The Name of the channel.
   * @param userId The id of the user.
   * @param topic The name of the topic.
   * @param meta The meta of the topic.
   * @param errorCode The error code.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onUser:(NSString * _Nonnull)userId
    joinTopic:(NSString * _Nonnull)topic
    inChannel:(NSString * _Nonnull)channelName
    withMeta:(NSData * _Nullable)meta
    result:(AgoraRtmStreamChannelErrorCode)errorCode;

/**
   * Occurs when user leave topic.
   *
   * @param channelName The Name of the channel.
   * @param userId The id of the user.
   * @param topic The name of the topic.
   * @param meta The meta of the topic.
   * @param errorCode The error code.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onUser:(NSString * _Nonnull)userId
    leaveTopic:(NSString * _Nonnull)topic
    inChannel:(NSString * _Nonnull)channelName
    withMeta:(NSData * _Nullable)meta
    result:(AgoraRtmStreamChannelErrorCode)errorCode;

/**
   * Occurs when user subscribe topic.
   *
   * @param channelName The Name of the channel.
   * @param userId The id of the user.
   * @param topic The name of the topic.
   * @param succeedUsers The subscribed users.
   * @param failedUser The failed to subscribe users.
   * @param errorCode The error code.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onUser:(NSString * _Nonnull)userId
    inTopic:(NSString * _Nonnull)topic
    inChannel:(NSString * _Nonnull)channelName
    withSubscribeSuccess:(NSArray<NSString *> * _Nonnull)succeedUsers
    withSubscribeFailed:(NSArray<NSString *> * _Nonnull)failedUsers
    result:(AgoraRtmStreamChannelErrorCode)errorCode;

/**
   * Occurs when user unsubscribe topic.
   *
   * @param channelName The Name of the channel.
   * @param userId The id of the user.
   * @param topic The name of the topic.
   * @param succeedUsers The unsubscribed users.
   * @param failedUser The failed to unsubscribe users.
   * @param errorCode The error code.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)rtmKit
    onUser:(NSString * _Nonnull)userId
    inTopic:(NSString * _Nonnull)topic
    inChannel:(NSString * _Nonnull)channelName
    withUnsubscribeSuccess:(NSArray<NSString *> * _Nonnull)succeedUsers
    withUnsubscribeFailed:(NSArray<NSString *> * _Nonnull)failedUsers
    result:(AgoraRtmStreamChannelErrorCode)errorCode;

/**
   * Occurs when the connection state changes between rtm sdk and agora service.
   *
   * @param channelName The Name of the channel.
   * @param state The new connection state.
   * @param reason The reason for the connection state change.
   */
- (void)rtmKit:(AgoraRtmClientKit * _Nonnull)kit
    channel:(NSString * _Nonnull)channelName
    connectionStateChanged:(AgoraRtmClientConnectionState)state
    result:(AgoraRtmClientConnectionChangeReason)reason;
    
@end


/**
 * The AgoraRtmClientKit class.
 *
 * This class provides the main methods that can be invoked by your app.
 *
 * AgoraRtmClientKit is the basic interface class of the Agora RTM SDK.
 * Creating an AgoraRtmClientKit object and then calling the methods of
 * this object enables you to use Agora RTM SDK's functionality.
 */
__attribute__((visibility("default"))) @interface AgoraRtmClientKit : NSObject

@property (atomic, weak, nullable) id<AgoraRtmClientDelegate> agoraRtmDelegate;

/**
 * Initializes the rtm client instance.
 *
 * @param [in] config The configurations for RTM Client.
 * @param [in] delegate  The callbacks handler.

 */
- (instancetype _Nullable) initWithConfig:(AgoraRtmClientConfig * _Nonnull)config
                                 delegate:(id <AgoraRtmClientDelegate> _Nullable)delegate;

/**
 * create a stream channel instance.
 *
 * @param [in] channelName The Name of the channel.
 */
- (AgoraRtmStreamChannel * _Nullable)createStreamChannel:(NSString * _Nonnull)channelName;


/**
 * destroy the rtm client instance.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int) destroy;
@end

__attribute__((visibility("default"))) @interface AgoraRtmStreamChannel : NSObject
/**
 * join the channel.
 *
 * @param [in] options join channel options.
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)joinWithOption:(AgoraRtmJoinChannelOption * _Nonnull) option;

/**
 * leave the channel.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int)leave;

/**
  * return the channel name of this stream channel.
  *
  * @return The channel name.
  */
- (NSString * _Nonnull) getChannelName;

/**
 * join a topic.
 *
 * @param [in] topic The name of the topic.
 * @param [in] options The options of create a topic.
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int) joinTopic:(NSString * _Nonnull)topic withOption:(AgoraRtmJoinTopicOption * _Nullable)option;

/**
   * publish a message in the topic.
   *
   * @param [in] topic The name of the topic.
   * @param [in] message The content of the message.
   * @return
   * - 0: Success.
   * - < 0: Failure.
   */
- (int) publishMessage:(NSData * _Nonnull) message
               inTopic:(NSString * _Nonnull) topic;

/**
 * leave the topic.
 *
 * @param [in] topic The name of the topic.
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int) leaveTopic:(NSString * _Nonnull)topic;

/**
 * unsubscribe a topic.
 *
 * @param [in] topic The name of the topic.
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int) subscribeTopic:(NSString * _Nonnull)topic withOption:(AgoraRtmTopicOption * _Nullable)option;

/**
 * unsubscribe a topic.
 *
 * @param [in] topic The name of the topic.
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int) unsubscribeTopic:(NSString * _Nonnull)topic withOption:(AgoraRtmTopicOption * _Nullable)option;

- (int) getSubscribedUserList:(NSMutableArray<NSString *> * _Nonnull)users inTopic:(NSString * _Nonnull)topic;

/**
 * release the stream channel instance.
 *
 * @return
 * - 0: Success.
 * - < 0: Failure.
 */
- (int) destroy;
@end
