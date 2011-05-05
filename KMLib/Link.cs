namespace KMLib
{
    using Abstract;

    public class Link : ALink
    {
    }

    /*
<Link id="ID">
  <!-- specific to Link -->
  <href>...</href>                      <!-- anyURI -->
  <refreshMode>onChange</refreshMode>   
    <!-- refreshModeEnum: onChange, OnInterval, or OnExpire -->   
  <refreshInterval>4</refreshInterval>  <!-- float -->
  <viewRefreshMode>never</viewRefreshMode> 
    <!-- viewRefreshModeEnum: never, onStop, onRequest, onRegion -->
  <viewRefreshTime>4</viewRefreshTime>  <!-- float -->
  <viewBoundScale>1</viewBoundScale>    <!-- float -->
  <viewFormat>BBOX=[bboxWest],[bboxSouth],[bboxEast],[bboxNorth]</viewFormat>
                                        <!-- string -->
  <httpQuery>...</httpQuery>            <!-- string -->
</Link>
     */
}