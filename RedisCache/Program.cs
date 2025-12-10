using RedisCache;

var practice = new RedisPracticeService();
await practice.PracticeStrings();
await practice.PracticeHashes();
await practice.PracticeLists();
await practice.PracticeSets();
await practice.PracticeSortedSets();
await practice.PracticeJson();
await practice.PracticeKeyOperations();
await practice.PracticeSearchKeys();