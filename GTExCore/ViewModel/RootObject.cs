
using BettingServiceReference;

public class State
{
    public int betDelay { get; set; }
    public bool bspReconciled { get; set; }
    public bool complete { get; set; }
    public bool inplay { get; set; }
    public int numberOfWinners { get; set; }
    public int numberOfRunners { get; set; }
    public int numberOfActiveRunners { get; set; }
    public string lastMatchTime { get; set; }
    public double totalMatched { get; set; }
    public double totalAvailable { get; set; }
    public bool crossMatching { get; set; }
    public bool runnersVoidable { get; set; }
    public long version { get; set; }
    public string status { get; set; }
}
public class Metadata
{
    public string SIRE_NAME { get; set; }
    public string CLOTH_NUMBER_ALPHA { get; set; }
    public object OFFICIAL_RATING { get; set; }
    public string COLOURS_DESCRIPTION { get; set; }
    public string COLOURS_FILENAME { get; set; }
    public string FORECASTPRICE_DENOMINATOR { get; set; }
    public string DAMSIRE_NAME { get; set; }
    public string WEIGHT_VALUE { get; set; }
    public string SEX_TYPE { get; set; }
    public string DAYS_SINCE_LAST_RUN { get; set; }
    public object WEARING { get; set; }
    public object OWNER_NAME { get; set; }
    public object DAM_YEAR_BORN { get; set; }
    public string SIRE_BRED { get; set; }
    public string JOCKEY_NAME { get; set; }
    public string DAM_BRED { get; set; }
    public object ADJUSTED_RATING { get; set; }
    public string runnerId { get; set; }
    public string CLOTH_NUMBER { get; set; }
    public object SIRE_YEAR_BORN { get; set; }
    public string TRAINER_NAME { get; set; }
    public string COLOUR_TYPE { get; set; }
    public string AGE { get; set; }
    public string DAMSIRE_BRED { get; set; }
    public object JOCKEY_CLAIM { get; set; }
    public string FORM { get; set; }
    public string FORECASTPRICE_NUMERATOR { get; set; }
    public string BRED { get; set; }
    public string DAM_NAME { get; set; }
    public object DAMSIRE_YEAR_BORN { get; set; }
    public string STALL_DRAW { get; set; }
    public string WEIGHT_UNITS { get; set; }
}
public class Description
{
    public string runnerName { get; set; }
    public Metadata metadata { get; set; }
}

public class State2
{
    public int sortPriority { get; set; }
    public double lastPriceTraded { get; set; }
    public double totalMatched { get; set; }
    public string status { get; set; }
    public double adjustmentFactor { get; set; }
}

public class AvailableToBack
{
    public double price { get; set; }
    public double size { get; set; }
}

public class AvailableToLay
{
    public double price { get; set; }
    public double size { get; set; }
}

public class Exchange
{
    public List<AvailableToBack> availableToBack { get; set; }
    public List<AvailableToLay> availableToLay { get; set; }
}

public class Runner
{
    public int selectionId { get; set; }
    public double handicap { get; set; }
    public Description description { get; set; }
    public State2 state { get; set; }
    public Exchange exchange { get; set; }
    public long ProfitandLoss { get; set; }
}

public class MarketNode
{
    public string marketId { get; set; }
    public bool isMarketDataDelayed { get; set; }
    public string highWaterMark { get; set; }
    public State state { get; set; }
    public List<Runner> runners { get; set; }
    public bool isMarketDataVirtual { get; set; }
    public decimal PoundRate { get; set; }
    public List<DebitCredit> DebitCredit { get; set; }
    public string MarketBookName { get; set; }
    public string FavoriteSelectionName { get; set; }
    public string FavoriteBack { get; set; }
    public string FavoriteLay { get; set; }
    public string FavoriteID { get; set; }
    public string MarketStatusstr { get; set; }
    public string FavoriteBackSize { get; set; }
    public string FavoriteLaySize { get; set; }
    public string OpenDate { get; set; }
    public DateTime OrignalOpenDate { get; set; }
    public string UserBetsEndUser { get; set; }
    public string UserBetsAgent { get; set; }
    public string UserBetsAdmin { get; set; }

    public string SheetName { get; set; }
    public string MainSportsname { get; set; }
}

public class EventNode
{
    public int eventId { get; set; }
    public List<MarketNode> marketNodes { get; set; }
}

public class EventType
{
    public int eventTypeId { get; set; }
    public List<EventNode> eventNodes { get; set; }
}

public class RootObject
{
    public string currencyCode { get; set; }
    public List<EventType> eventTypes { get; set; }
    public bool indicative { get; set; }
}
