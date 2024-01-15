Feature: StockMarketMatchingEngineFeature2

A short summary of the feature

Scenario Outline: Enqueue SellOrder

	Given Order 'SellOrder' Has Been Defined
		| Side   | Price   | Amount   | IsFillAndKill   | ExpireTime   | MarketId   |
		| <Sell> | <Price> | <Amount> | <IsFillAndKill> | <ExpireTime> | <MarketId> |

	When I Register The Order 'SellOrder'

	Then Order 'SellOrder' Should Be Enqueued

Examples:
	| Sell | Price | Amount | IsFillAndKill | ExpireTime | MarketId                             |
	| 0    | 300   | 15     | false         | 2024-02-05 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |
	| 0    | 400   | 10     | false         | 2024-02-05 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |
	| 0    | 500   | 10     | false         | 2024-02-05 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |

Scenario Outline: TradeOrders

	Given Order 'SellOrder' Has Been Registered
		| Side | Price | Amount | IsFillAndKill | ExpireTime                  | MarketId                             |
		| 0    | 100   | 5      | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |


	And Order 'BuyOrder' Has Been Defined
		| Side  | Price   | Amount   | IsFillAndKill   | ExpireTime   | MarketId   |
		| <Buy> | <Price> | <Amount> | <IsFillAndKill> | <ExpireTime> | <MarketId> |

	When I Register The Order 'BuyOrder'


	Then The following 'Trade' will be created
		| BuyOrderId   | SellOrderId   | Amount        | Price        |
		| <BuyOrderId> | <SellOrderId> | <TradeAmount> | <TradePrice> |

		
	And Order 'BuyOrder' Should Be Modified  like this
		| Side  | Price   | Amount           | IsFillAndKill | ExpireTime                  |
		| <Buy> | <Price> | <ModifiedAmount> | false         | 2024-02-05 09:30:26.2080000 |

Examples:
	| Buy | Price | Amount | IsFillAndKill | ExpireTime                  | MarketId                             | BuyOrderId | SellOrderId | TradeAmount | TradePrice | ModifiedAmount |
	| 1   | 100   | 5      | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 | 2          | 1           | 5           | 100        | 0              |


Scenario Outline: ModifyOrder
	Given Order 'SellOrder' Has Been Registered
		| Side   | Price   | Amount   | IsFillAndKill   | ExpireTime   | MarketId   |
		| <Sell> | <Price> | <Amount> | <IsFillAndKill> | <ExpireTime> | <MarketId> |
	
	And Order 'ModifiedOrder' Has Been Defined
		| Side | Price | Amount | IsFillAndKill | ExpireTime                  | MarketId                             |
		| 0    | 1000  | 10     | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |
	
	When I Modify The Order 'SellOrder' to 'ModifiedOrder'


	Then The order 'SellOrder'  Should Be Found like 'ModifiedOrder'


Examples:
	| Sell | Price | Amount | IsFillAndKill | ExpireTime                  | MarketId                             |
	| 0    | 1000  | 1000   | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |



Scenario Outline: CancelOrder
	Given Order 'SellOrder' Has Been Registered
		| Side   | Price   | Amount   | IsFillAndKill   | ExpireTime   | MarketId   |
		| <Sell> | <Price> | <Amount> | <IsFillAndKill> | <ExpireTime> | <MarketId> |

	When I cancel 'SellOrder'

	Then The order 'SellOrder'  Should Be Canceled


Examples:
	| Sell | Price | Amount | IsFillAndKill | ExpireTime                  | MarketId                             |
	| 0    | 100   | 5      | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |
	| 0    | 200   | 5      | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |
	| 0    | 300   | 5      | false         | 2024-02-05 09:30:26.2080000 | 74c666f8-432c-4534-bdcb-a0e8b51ab5e5 |




	
