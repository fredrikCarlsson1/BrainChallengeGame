using System.Collections;
using System.Collections.Generic;

public class WordSplitClass
{

    public string[] splitAllWordsToArray()
    {
        string[] threeWordArray = threeWords.Split();
        return threeWordArray;
    }

    private string threeWords = "abc bad cox dag ebb fal gag hag ide jaa " +
        "kal lad mad nar ack bag cup dal ecu fan gaj haj ido jag kam lag " +
        "maj nav adb bah dam eda far gal hak ids jak kan lam mak ned aga bak " +
        " dan eeg fas gam hal ila jam kap lan mal neg " +
        "agg bal dar egg fat gan han ils jan kar lar man nej " +
        "agn ban ddt ego fax gap har ina jap kas lar mar nek " +
        "ags bar deg eja feg gas has ini jas kat lat mas ner " +
        "aha bas dej eka fel gav hat ink jet kav lav mat nia " +
        "air bed del eke fem gay hav isa jod ked lax max nid " +
        "akt bel dem ekg fen gel hbt ism jon kel led mbl nie " +
        "ale ben den eko fes gem hed itu joo kex lej med nig " +
        "alf ber det eks fet gen hej ity jox kid lek mej nio " +
        "alg bes dia eld fez ger hel jul kik lem men nit " +
        "all bet dig els fia ges hem jäs kil len mer nix " +
        "alm bil din emu fik get hen jäv kip ler mes nja " +
        "aln bin dip ena fil gig hes kir les mig nod " +
        "alp bio dis ene fin gin het kis lev mil nog " +
        "als bis dit ens fis gip hin kit lid mim nor " +
        "alt bit dna eon fix gir hit kiv lie min nos " +
        "alv bli doa era flo giv hiv kli lik mix not " +
        "amt bly dog ers fly gli hoa klo lim mms nus " +
        "ana blå dok ert flå glo hoj klå lin mod nya " +
        "and boa dom ess fog gno hon klä lip moj nye " +
        "ann bob don est fon gnu hop knä lir mol nyp " +
        "ans bod dop ett for gny hor kod lit mon nys " +
        "apa bog dos eva fot goa hos koj liv mor nåd " +
        "ara boj dov exa fri god hot kok lix mos nål " +
        "arg bok dra fru goj hov kol lob mot nån " +
        "ark bol dua frö gol hud kom lod mua når " +
        "arm bom duk ful gom huj kon log mun nås" +
        "arr bop dum fur got huk kor loj mur nåt " +
        "ars bor dun fux gro hum kos lok mus när " +
        "art bos duo fyr gry hur kry lom myr näs " +
        "arv bot dur fåk grå hus kub lon mys nät " +
        "asa bov dus fån gud hut kuf los myt nää " +
        "ask box dvd får gul huv kuk lov måg nöd " +
        "asp bra dyk fås guy hyf kul lue mål nöj " +
        "ass bre dyn fän gym hyl kur lur mån nöp " +
        "att bro dyr fäs går hyn kut lus mår nös " +
        "ave bry dys fät gås hyr kyl lut mås nöt " +
        "avi bua dåd föd gåt hys kyp lux män " +
        "axa bud dån föl gäl håg kåk lya mär " +
        "buk dän fön gäv hål kål lyd mät" +
        "bur där för göd hån kår lys mön" +
        "bus död fös gök hår kåt lyx mör " +
        "byk dög göl håv käk låd mös " +
        "byn dök göm häl käl låd möt " +
        "bys döm gör hän kär lån " +
        "byt dön gös här käx lår " +
        "båg döp göt häv köa lås " +
        "båk dör hög kök låt " +
        "bål dös höj köl läa  " +
        "bår döv hök kön läk  " +
        "bås hör köp läm " +
        "båt hös kör län " +
        "bär kös lär bög läs böj lät bök löd böl lök bön lön bör löp böt lös löt löv " +
        "oas paj rad sak tag udd vad yen zon åar äga öar " +
        "obs par rai sal tak uer vag yla zoo åbo ägd öbo " +
        "och pax rak sam tal uff vaj yls åda ägg öda ock peg ram sam tam ufo vak ymp ådi ägo öde" +
        "ode pek rap sav tar ugn val ynk åhå ägs öga " +
        "oja pen rar sax tas ula van yra åjo ägt öka " +
        "oks pep ras sch tax ulk var yrt åka älg öks " +
        "olm per rav sed tbc ull vas yta åks älv öla " +
        "oms pet red seg tee ulv vax yvs åkt äng öls " +
        "ona pik rek sel teg ung ved yxa åla ära ömt " +
        "ond pil rem sen ten uns vek åls ärg öns " +
        "ont pin ren ser ter unt vem åma ärm öra " +
        "opp pip rep ses tes upp ven åns ärr öre " +
        "ord pir res set tia urs vet åra ärt örn " +
        "ork pis rev sex tid ute vev års ärv örs " +
        "orm pix ria sia tie uti via åse äss ört " +
        "oro pli rid sid tig utö vid åta äta ösa " +
        "ors pol rik sig tik uvs vig åts äts öst " +
        "ort pop rim sik tio vik ätt ött " +
        "orv por ris sil tir vin öva " +
        "osa pro rit sim tja vis " +
        "oss pst riv sin tji vit " +
        "ost pub roa sir tjo viv " +
        "oxe puh rob sis toa vom " +
        "oår pur rom sju tog vov " +
        "oöm pyr ron sjå tok vrå " +
        "pys rop sjö tom vyn " +
        "påg ror ska ton vys " +
        "påk ros ske tre våd " +
        "påt rot sko tri våg " +
        "pär rov sky tro våm " +
        "pöl rum sly try våp " +
        "pös rus slå trå vår " +
        "rya slö trä våt " +
        "ryk sms tub väg " +
        "ryl små tum väj " +
        "rym sno tun väl " +
        "rys snö tur vän " +
        "ryt sol tus väs " +
        "råa som tut vät " +
        "råd son tvi väv " +
        "råe sop två väx " +
        "råg sos tya " +
        "råk sot tyd " +
        "råm sov tyg " +
        "rån spa typ " +
        "rår spe tyr "  +
        "rås spy tåa " +
        "räd spå tåg " +
        "räl spä tål " +
        "rät spö tån " +
        "räv sta tår " +
        "röd sto tås " +
        "röj stå tåt " +
        "rök stä tär " +
        "rön sug tät " +
        "rör sup täv " +
        "rör sur töa rea " +
        "röt sus töj röv suv töm syd tör syl tös syn syo syr sys såg sån sår sås såt såå säd säg säj säl sär säv sög sök söl söm söp söt söv";



}


